using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MediatR;
using CourseStudio.Doamin.Models.Playlists;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Doamin.Models.Identities;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Events;
using CourseStudio.Domain.Persistence.Mappings.Identities;
using CourseStudio.Domain.Persistence.Mappings.Courses;
using CourseStudio.Domain.Persistence.Mappings.Playlists;
using CourseStudio.Domain.Persistence.Mappings.Trades;
using CourseStudio.Domain.Persistence.Mappings.CourseAttributes;
using CourseStudio.Domain.Persistence.Mappings.Events;
using CourseStudio.Domain.Persistence.Mappings.Users;
using CourseStudio.Lib.Utilities.Database;

namespace CourseStudio.Domain.Persistence
{
    public class CourseContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        private readonly IDatabaseConnectionHelper _databaseConnectionHelper;
        private readonly IMediator _mediator;

        public CourseContext(
            DbContextOptions<CourseContext> options,
            IDatabaseConnectionHelper databaseConnectionHelper,
            IMediator mediator
        ) : base(options)
        {
            _mediator = mediator;
            _databaseConnectionHelper = databaseConnectionHelper;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_databaseConnectionHelper.ConnectionString);
        }

		public DbSet<IdentityToken> IdentityTokens { get; set; }
		public DbSet<IdentityTokenBlacklist> IdentityTokenBlacklists { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAttribute> CourseAttributes { get; set; }
        public DbSet<EntityAttributeType> EntityAttributeTypes { get; set; }
        public DbSet<EntityAttribute> EntityAttributes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<Video> Videos { get; set; }
		public DbSet<Link> Links { get; set; }
		public DbSet<Presentation> Presentations { get; set; }
		public DbSet<Document> Documents { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistCourse> PlaylistCourses { get; set; }
        public DbSet<CourseAuditing> CourseAuditings { get; set; }
        public DbSet<TutorAuditing> TutorAuditings { get; set; }
		public DbSet<ApplicationUserCourse> ApplicationUserCourses { get; set; }
		public DbSet<StudyRecord> StudyRecords { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCoupon> OrderCoupons { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<LineItem> LineItems { get; set; }
		public DbSet<LineCoupon> LineCoupons { get; set; }
		public DbSet<PaymentProfile> PaymentProfiles { get; set; }
        public DbSet<TransactionRecord> TransactionRecords { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponRule> CouponRules { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<CouponUser> CouponUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identities
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new RoleClaimMap());
            builder.ApplyConfiguration(new UserRoleMap());
			builder.ApplyConfiguration(new UserClaimMap());
			builder.ApplyConfiguration(new IdentityTokenMap());
			builder.ApplyConfiguration(new IdentityTokenBlacklistMap());
			builder.ApplyConfiguration(new VendorLoginMap());
			builder.ApplyConfiguration(new VendorTokenMap());

            // Users
			builder.ApplyConfiguration(new ApplicationUserMap());
            builder.ApplyConfiguration(new TutorMap());
			builder.ApplyConfiguration(new TutorAuditingMap());
			builder.ApplyConfiguration(new AdministratorMap());

            //Course
			builder.ApplyConfiguration(new CourseMap());
			builder.ApplyConfiguration(new ReviewMap());
			builder.ApplyConfiguration(new LikeMap());
            builder.ApplyConfiguration(new SectionMap());
            builder.ApplyConfiguration(new LectureMap());
			builder.ApplyConfiguration(new ContentMap());
			builder.ApplyConfiguration(new LinkMap());
			builder.ApplyConfiguration(new PresentationMap());
			builder.ApplyConfiguration(new DocumentMap());
            builder.ApplyConfiguration(new VideoMap());
            builder.ApplyConfiguration(new CourseAttributeMap());
            builder.ApplyConfiguration(new EntityAttributeMap());
            builder.ApplyConfiguration(new EntityAttributeTypeMap());
            builder.ApplyConfiguration(new PlaylistMap());
            builder.ApplyConfiguration(new PlaylistCourseMap());
			builder.ApplyConfiguration(new UserCourseMap());
            builder.ApplyConfiguration(new StudyRecordMap());
            builder.ApplyConfiguration(new CourseAuditingMap());

            //Orders
            builder.ApplyConfiguration(new OrderMap());
            builder.ApplyConfiguration(new ShoppingCartMap());
			builder.ApplyConfiguration(new LineItemMap());
			builder.ApplyConfiguration(new LineCouponMap());
			builder.ApplyConfiguration(new PaymentProfileMap());
            builder.ApplyConfiguration(new TransactionRecordMap());
            builder.ApplyConfiguration(new OrderCouponMap());

            // Events
            builder.ApplyConfiguration(new EventMap());

            // Coupons
            builder.ApplyConfiguration(new CouponMap());
            builder.ApplyConfiguration(new CouponRuleMap());
            builder.ApplyConfiguration(new ScopeMap());
            builder.ApplyConfiguration(new CouponUserMap());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed thought the DbContext will be commited
            await base.SaveChangesAsync();

            return true;
        }

    }
}
