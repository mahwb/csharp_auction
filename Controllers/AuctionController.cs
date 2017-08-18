using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using csharp_belt.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace csharp_belt.Controllers
{
    public class AuctionController : Controller
    {
        private BeltContext _context;
 
        public AuctionController(BeltContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("auctions")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("userid") == null) {
                return RedirectToAction("LoginPage", "Login");
            }
            List<Auction> auctions = _context.auctions.Include(a => a.User).OrderBy(a => a.EndDate).ToList();
            foreach (var auction in auctions) {
                if ((auction.EndDate - DateTime.Now).TotalDays <= 0) {
                    List<Bid> bids = _context.bids.Where(b => b.AuctionId == auction.AuctionId).ToList();
                    foreach (var bid in bids) {
                        //found highest bidder
                        if (bid.Amount == auction.Bid) {
                            //give money to auction creator
                            Wallet auction_wallet = _context.wallets.SingleOrDefault(w => w.UserId == auction.UserId);
                            auction_wallet.Amount += bid.Amount;
                        } else {
                            //give money back to bidder
                            Wallet bidder_wallet = _context.wallets.SingleOrDefault(w => w.UserId == bid.UserId);
                            bidder_wallet.Amount += bid.Amount;
                        }
                        _context.bids.Remove(bid);
                    }
                    _context.auctions.Remove(auction);
                    _context.SaveChanges();
                }
            }
            ViewBag.Auctions = _context.auctions.Include(a => a.User).OrderBy(a => a.EndDate).ToList();
            ViewBag.UserId = HttpContext.Session.GetInt32("userid");
            ViewBag.Wallet = _context.wallets.Include(w => w.User).SingleOrDefault(w => w.UserId == HttpContext.Session.GetInt32("userid"));       
            return View();
        }

        [HttpGet]
        [Route("new")]
        public IActionResult NewPage()
        {
            if (HttpContext.Session.GetInt32("userid") == null) {
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }


        [HttpGet]
        [Route("process")]
        public IActionResult Process()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process(AuctionCheck auctioninfo)
        {
            if (HttpContext.Session.GetInt32("userid") == null) {
                return RedirectToAction("LoginPage", "Login");
            }
            if (ModelState.IsValid) {
                Auction newauction = new Auction {
                    Name = auctioninfo.Name,
                    Description = auctioninfo.Description,
                    Bid = auctioninfo.Bid,
                    EndDate = auctioninfo.EndDate,
                    UserId = (int)HttpContext.Session.GetInt32("userid"),
                    CreatedAt = DateTime.Now,
                    UploadedAt = DateTime.Now,
                };
                _context.auctions.Add(newauction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("NewPage");
        }

        [HttpGet]
        [Route("info/{id}")]
        public IActionResult InfoPage(int id)
        {
            if (HttpContext.Session.GetInt32("userid") == null) {
                return RedirectToAction("LoginPage", "Login");
            }
            Auction auction = _context.auctions.Include(a => a.User).SingleOrDefault(a => a.AuctionId == id);
            Bid bidder = _context.bids.Include(b => b.User).SingleOrDefault(b => b.AuctionId == id && b.Amount == auction.Bid);
            ViewBag.Auction = auction;
            ViewBag.Bidder = bidder;
            ViewBag.Error = TempData["error"];
            return View();
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("userid") == null) {
                return RedirectToAction("LoginPage", "Login");
            }
            Auction delauction = _context.auctions.SingleOrDefault(a => a.AuctionId == id);
            List<Bid> delbids = _context.bids.Where(b => b.AuctionId == id).ToList();
            if (delauction.UserId == HttpContext.Session.GetInt32("userid")) {
                _context.auctions.Remove(delauction);
                foreach (var bid in delbids) {
                    _context.bids.Remove(bid);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("newbid/{id}")]
        public IActionResult NewBid(int id, double Amount)
        {
            if (HttpContext.Session.GetInt32("userid") == null) {
                return RedirectToAction("LoginPage", "Login");
            }
            Auction auction = _context.auctions.SingleOrDefault(a => a.AuctionId == id);
            Wallet wallet = _context.wallets.SingleOrDefault(w => w.UserId == HttpContext.Session.GetInt32("userid"));
            if (Amount > auction.Bid) {
                if (wallet.Amount >= Amount) {
                    Bid newbid = new Bid {
                        AuctionId = auction.AuctionId,
                        UserId = (int)HttpContext.Session.GetInt32("userid"),
                        Amount = Amount,
                        CreatedAt = DateTime.Now,
                        UploadedAt = DateTime.Now,
                    };
                    _context.bids.Add(newbid);
                    auction.Bid = Amount;
                    //take money from wallet
                    wallet.Amount -= Amount;
                    _context.SaveChanges();
                } else {
                    TempData["error"] = "Not enough money.";
                }
            } else {
                TempData["error"] = "Bid amount is too small.";
            }
            return RedirectToAction("InfoPage", new { id = id });
        }
    }
}