﻿// <auto-generated />
using System;
using APBD_Projekt.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APBD_Projekt.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APBD_Projekt.Models.Category", b =>
                {
                    b.Property<int>("IdCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategory"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdCategory");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdClient"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.HasKey("IdClient");

                    b.ToTable("Clients");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Client");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("APBD_Projekt.Models.Contract", b =>
                {
                    b.Property<int>("IdContract")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContract"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("FinalPrice")
                        .HasColumnType("money");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int?>("IdDiscount")
                        .HasColumnType("int");

                    b.Property<int>("IdSoftwareVersion")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SignedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<int>("YearsOfSupport")
                        .HasColumnType("int");

                    b.HasKey("IdContract");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdDiscount");

                    b.HasIndex("IdSoftwareVersion");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("APBD_Projekt.Models.ContractPayment", b =>
                {
                    b.Property<int>("IdContractPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContractPayment"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("IdContract")
                        .HasColumnType("int");

                    b.Property<decimal>("PaymentAmount")
                        .HasColumnType("money");

                    b.HasKey("IdContractPayment");

                    b.HasIndex("IdContract");

                    b.ToTable("ContractPayments");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Discount", b =>
                {
                    b.Property<int>("IdDiscount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDiscount"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("IdDiscount");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Role", b =>
                {
                    b.Property<int>("IdRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRole"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdRole");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Software", b =>
                {
                    b.Property<int>("IdSoftware")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSoftware"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<decimal>("YearlyLicensePrice")
                        .HasColumnType("money");

                    b.HasKey("IdSoftware");

                    b.ToTable("Software");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SoftwareVersion", b =>
                {
                    b.Property<int>("IdSoftwareVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSoftwareVersion"));

                    b.Property<int>("IdSoftware")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("IdSoftwareVersion");

                    b.HasIndex("IdSoftware");

                    b.ToTable("SoftwareVersions");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Subscription", b =>
                {
                    b.Property<int>("IdSubscription")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscription"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int?>("IdDiscount")
                        .HasColumnType("int");

                    b.Property<int>("IdSubscriptionOffer")
                        .HasColumnType("int");

                    b.Property<DateTime>("NextPaymentDueDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("ShouldApplyRegularClientDiscount")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("IdSubscription");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdDiscount");

                    b.HasIndex("IdSubscriptionOffer");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionOffer", b =>
                {
                    b.Property<int>("IdSubscriptionOffer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscriptionOffer"));

                    b.Property<int>("IdSoftware")
                        .HasColumnType("int");

                    b.Property<int>("MonthsPerRenewalTime")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.HasKey("IdSubscriptionOffer");

                    b.HasIndex("IdSoftware");

                    b.ToTable("SubscriptionOffers");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionPayment", b =>
                {
                    b.Property<int>("IdSubscriptionPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscriptionPayment"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("IdSubscription")
                        .HasColumnType("int");

                    b.HasKey("IdSubscriptionPayment");

                    b.HasIndex("IdSubscription");

                    b.ToTable("SubscriptionPayments");
                });

            modelBuilder.Entity("APBD_Projekt.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<int>("IdRole")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime?>("RefreshTokenExp")
                        .IsRequired()
                        .HasColumnType("datetime");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("IdUser");

                    b.HasIndex("IdRole");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CategorySoftware", b =>
                {
                    b.Property<int>("CategoriesIdCategory")
                        .HasColumnType("int");

                    b.Property<int>("SoftwareIdSoftware")
                        .HasColumnType("int");

                    b.HasKey("CategoriesIdCategory", "SoftwareIdSoftware");

                    b.HasIndex("SoftwareIdSoftware");

                    b.ToTable("CategorySoftware");
                });

            modelBuilder.Entity("APBD_Projekt.Models.CompanyClient", b =>
                {
                    b.HasBaseType("APBD_Projekt.Models.Client");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("KRS")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasDiscriminator().HasValue("CompanyClient");
                });

            modelBuilder.Entity("APBD_Projekt.Models.IndividualClient", b =>
                {
                    b.HasBaseType("APBD_Projekt.Models.Client");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PESEL")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasDiscriminator().HasValue("IndividualClient");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Contract", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Client", "Client")
                        .WithMany("Contracts")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBD_Projekt.Models.Discount", "Discount")
                        .WithMany("Contracts")
                        .HasForeignKey("IdDiscount");

                    b.HasOne("APBD_Projekt.Models.SoftwareVersion", "SoftwareVersion")
                        .WithMany("Contracts")
                        .HasForeignKey("IdSoftwareVersion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Discount");

                    b.Navigation("SoftwareVersion");
                });

            modelBuilder.Entity("APBD_Projekt.Models.ContractPayment", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Contract", "Contract")
                        .WithMany("ContractPayments")
                        .HasForeignKey("IdContract")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SoftwareVersion", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Software", "Software")
                        .WithMany("SoftwareVersions")
                        .HasForeignKey("IdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Subscription", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Client", "Client")
                        .WithMany("Subscriptions")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBD_Projekt.Models.Discount", "Discount")
                        .WithMany("Subscriptions")
                        .HasForeignKey("IdDiscount");

                    b.HasOne("APBD_Projekt.Models.SubscriptionOffer", "SubscriptionOffer")
                        .WithMany("Subscriptions")
                        .HasForeignKey("IdSubscriptionOffer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Discount");

                    b.Navigation("SubscriptionOffer");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionOffer", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Software", "Software")
                        .WithMany("SubscriptionOffers")
                        .HasForeignKey("IdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionPayment", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Subscription", "Subscription")
                        .WithMany("SubscriptionPayments")
                        .HasForeignKey("IdSubscription")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("APBD_Projekt.Models.User", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CategorySoftware", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesIdCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBD_Projekt.Models.Software", null)
                        .WithMany()
                        .HasForeignKey("SoftwareIdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("APBD_Projekt.Models.Client", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Contract", b =>
                {
                    b.Navigation("ContractPayments");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Discount", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Software", b =>
                {
                    b.Navigation("SoftwareVersions");

                    b.Navigation("SubscriptionOffers");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SoftwareVersion", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Subscription", b =>
                {
                    b.Navigation("SubscriptionPayments");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionOffer", b =>
                {
                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
