using Microsoft.EntityFrameworkCore;
 
namespace csharp_belt.Models
{
    public class BeltContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }

	// This DbSet contains "Person" objects and is called "Users"
    	public DbSet<User> users { get; set; }
    	public DbSet<Wallet> wallets { get; set; }
    	public DbSet<Auction> auctions { get; set; }
    	public DbSet<Bid> bids { get; set; }
    }
}