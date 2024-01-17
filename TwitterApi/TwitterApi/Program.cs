using BLL.Services;
using BLL.Services.IServices;
using DAL.DataContext;
using DAL.Repository;
using DAL.Repository.IRepository;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddDbContext<TwitterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Add dependency injection
//Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IFollowingListRepository, FollowingListRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFollowingListService, FollowingListService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

//app.UseCors(options => options
//    .WithOrigins(new[] { "https://localhost:3000", "https://localhost:8000", "https://localhost:4200", "http://localhost:5173" })
//    .AllowAnyHeader()
//    .AllowAnyMethod()
//    .AllowCredentials());

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000") // Add other origins as needed
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
