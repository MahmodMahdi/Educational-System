using BusinessLogicLayer.Services.CourseService;
using BusinessLogicLayer.Services.DepartmentService;
using BusinessLogicLayer.Services.InstructorService;
using BusinessLogicLayer.Services.RoleService;
using BusinessLogicLayer.Services.TraineeCoursesResultsService;
using BusinessLogicLayer.Services.TraineeService;
using BusinessLogicLayer.Services.UserService;
using DataAccessLayer.Authentication;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.CourseRepo;
using DataAccessLayer.Repositories.DepartmentRepo;
using DataAccessLayer.Repositories.InstructorRepo;
using DataAccessLayer.Repositories.RoleRepo;
using DataAccessLayer.Repositories.TraineeCoursesResultsRepo;
using DataAccessLayer.Repositories.TraineeRepo;
using DataAccessLayer.Repositories.UserRepo;
using DataAccessLayer.Seeding;
using DataAccessLayer.UnitofWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Educational_Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #region Context
            var connString = builder.Configuration.GetConnectionString("DB");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connString));
            #endregion
            #region Services
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ITraineeService, TraineeService>();
            builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<ICourseResultService, CourseResultService>();
            builder.Services.AddScoped<ICourseResultRepository, CourseResultRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion
            #region Identity Service
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                }
            )
                .AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleSeeding.SeedAsync(roleManager);
                UserSeeding.SeedAsync(userManager);
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}