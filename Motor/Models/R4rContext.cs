using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Motor.Models;

public partial class R4rContext : DbContext
{
    public R4rContext()
    {
    }

    public R4rContext(DbContextOptions<R4rContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<MotorModel> Motors { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<TypeMotor> Types { get; set; }

    public virtual DbSet<imgMotor> ImgMotors { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetials { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    private string host = Environment.GetEnvironmentVariable("PGHOST");


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql("Host=containers-us-west-15.railway.app;Port=6613;Database=railway;Username=postgres;Password=P1uIYcTfSal2qMZqwZzX");

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MotorModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("motor_pkey");

            entity.ToTable("motor");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.Createdby)
                .HasMaxLength(255)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasMaxLength(255)
                .HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.farmous).HasColumnName("famous");
            entity.Property(e => e.salePrice)
                .HasMaxLength(255)
                .HasColumnName("sale_price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TypeMotor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_pkey");

            entity.ToTable("type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
        });

        modelBuilder.Entity<imgMotor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("imgmotor_pkey");

            entity.ToTable("imgmotor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.idMotor)
                .HasColumnName("idmotor");
            entity.Property(e => e.Imgbase64)
                .HasColumnName("imgbase64");
        });
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("cartitem_pkey");

            entity.ToTable("cart_shop");

            entity.Property(e => e.CartId).HasColumnName("cartid");
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");
            entity.Property(e => e.motorId)
                .HasColumnName("motorId");
            entity.Property(e => e.createBy)
                .HasColumnName("createby");
            entity.Property(e => e.totalprice)
                .HasColumnName("totalprice");
            entity.Property(e => e.DateCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datecreated");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.orderId).HasName("order_pkey");

            entity.ToTable("order");

            entity.Property(e => e.orderId).HasColumnName("orderId");
            entity.Property(e => e.Createdby)
                .HasColumnName("createdby");
            entity.Property(e => e.Status)
                .HasColumnName("status");
            entity.Property(e => e.totalPrice)
                .HasColumnName("totalprice");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_detial_pkey");
            entity.ToTable("order_detial");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.orderId).HasColumnName("orderId");
            entity.Property(e => e.motorId)
                .HasColumnName("motorId");
            entity.Property(e => e.price)
                .HasColumnName("price");
            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("motor_cmt_pkey");

            entity.ToTable("motor_cmt");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdby)
                .HasColumnName("createdby");
            entity.Property(e => e.motorId)
                .HasColumnName("motor_id");
            entity.Property(e => e.comment)
                .HasColumnName("comment");
            entity.Property(e => e.createdDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdate");
            entity.Property(e => e.modifyDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifydate");
        });


        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("blog_pkey");

            entity.ToTable("blog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdby)
                .HasColumnName("created_by");
            entity.Property(e => e.Title)
                .HasColumnName("title");
            entity.Property(e => e.Content)
                .HasColumnName("content");
            entity.Property(e => e.Img)
                .HasColumnName("img");
            entity.Property(e => e.createdDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.modifyDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modify_date");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
