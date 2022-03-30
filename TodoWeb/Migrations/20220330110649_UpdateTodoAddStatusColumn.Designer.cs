﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoWeb.Data;

#nullable disable

namespace TodoWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220330110649_UpdateTodoAddStatusColumn")]
    partial class UpdateTodoAddStatusColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersInRoleId")
                        .HasColumnType("int");

                    b.HasKey("RolesId", "UsersInRoleId");

                    b.HasIndex("UsersInRoleId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("TodoListUser", b =>
                {
                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("TodoListsId")
                        .HasColumnType("int");

                    b.HasKey("AuthorsId", "TodoListsId");

                    b.HasIndex("TodoListsId");

                    b.ToTable("TodoListUser");
                });

            modelBuilder.Entity("TodoWeb.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TodoWeb.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TodoListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("TodoWeb.Models.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TodoListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("StatusId");

                    b.HasIndex("TodoListId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("TodoWeb.Models.TodoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("TodoWeb.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "EmailIndex")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("TodoWeb.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoWeb.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersInRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TodoListUser", b =>
                {
                    b.HasOne("TodoWeb.Models.User", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoWeb.Models.TodoList", null)
                        .WithMany()
                        .HasForeignKey("TodoListsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TodoWeb.Models.Status", b =>
                {
                    b.HasOne("TodoWeb.Models.TodoList", "TodoList")
                        .WithMany("Statuses")
                        .HasForeignKey("TodoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TodoList");
                });

            modelBuilder.Entity("TodoWeb.Models.Todo", b =>
                {
                    b.HasOne("TodoWeb.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("TodoWeb.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("TodoWeb.Models.TodoList", "TodoList")
                        .WithMany("Todos")
                        .HasForeignKey("TodoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Status");

                    b.Navigation("TodoList");
                });

            modelBuilder.Entity("TodoWeb.Models.TodoList", b =>
                {
                    b.Navigation("Statuses");

                    b.Navigation("Todos");
                });
#pragma warning restore 612, 618
        }
    }
}
