﻿// <auto-generated />
using System;
using APBD_Projekt.Context;
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
                        .HasColumnType("int")
                        .HasColumnName("IdCategory");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategory"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Name");

                    b.HasKey("IdCategory");

                    b.ToTable("Category");
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
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("Address");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.HasKey("IdClient");

                    b.ToTable("Client");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("APBD_Projekt.Models.Contract", b =>
                {
                    b.Property<int>("IdContract")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdContract");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContract"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("EndDate");

                    b.Property<decimal>("FinalPrice")
                        .HasColumnType("money")
                        .HasColumnName("FinalPrice");

                    b.Property<int>("IdClient")
                        .HasColumnType("int")
                        .HasColumnName("IdClient");

                    b.Property<int?>("IdDiscount")
                        .HasColumnType("int")
                        .HasColumnName("IdDiscount");

                    b.Property<int>("IdSoftwareVersion")
                        .HasColumnType("int")
                        .HasColumnName("IdSoftwareVersion");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("StartDate");

                    b.Property<int>("YearsOfSupport")
                        .HasColumnType("int")
                        .HasColumnName("YearsOfSupport");

                    b.HasKey("IdContract");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdDiscount");

                    b.HasIndex("IdSoftwareVersion");

                    b.ToTable("Contract");
                });

            modelBuilder.Entity("APBD_Projekt.Models.ContractPayment", b =>
                {
                    b.Property<int>("IdContractPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ContractPayment");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdContractPayment"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("DateTime");

                    b.Property<int>("IdContract")
                        .HasColumnType("int")
                        .HasColumnName("IdContract");

                    b.Property<decimal>("PaymentAmount")
                        .HasColumnType("money")
                        .HasColumnName("PaymentAmount");

                    b.HasKey("IdContractPayment");

                    b.HasIndex("IdContract");

                    b.ToTable("ContractPayment");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Discount", b =>
                {
                    b.Property<int>("IdDiscount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdDiscount");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDiscount"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("EndDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("Name");

                    b.Property<int>("Percentage")
                        .HasColumnType("int")
                        .HasColumnName("Percentage");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("StartDate");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("IdDiscount");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("APBD_Projekt.Models.RenewalTime", b =>
                {
                    b.Property<int>("IdRenewalTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdRenewalTime");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRenewalTime"));

                    b.Property<int>("Months")
                        .HasColumnType("int")
                        .HasColumnName("Months");

                    b.Property<int>("Years")
                        .HasColumnType("int")
                        .HasColumnName("Years");

                    b.HasKey("IdRenewalTime");

                    b.ToTable("RenewalTime");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Role", b =>
                {
                    b.Property<int>("IdRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdRole");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRole"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Name");

                    b.HasKey("IdRole");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Software", b =>
                {
                    b.Property<int>("IdSoftware")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSoftware");

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
                        .HasColumnType("money")
                        .HasColumnName("YearlyLicensePrice");

                    b.HasKey("IdSoftware");

                    b.ToTable("Software");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SoftwareVersion", b =>
                {
                    b.Property<int>("IdSoftwareVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSoftwareVersion");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSoftwareVersion"));

                    b.Property<int>("IdSoftware")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("IdSoftwareVersion");

                    b.HasIndex("IdSoftware");

                    b.ToTable("SoftwareVersion");
                });

            modelBuilder.Entity("APBD_Projekt.Models.Subscription", b =>
                {
                    b.Property<int>("IdSubscription")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSubscription");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscription"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("EndDate");

                    b.Property<int>("IdClient")
                        .HasColumnType("int")
                        .HasColumnName("IdClient");

                    b.Property<int?>("IdDiscount")
                        .HasColumnType("int")
                        .HasColumnName("IdDiscount");

                    b.Property<int>("IdSubscriptionOffer")
                        .HasColumnType("int")
                        .HasColumnName("IdSubscriptionOffer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("StartDate");

                    b.HasKey("IdSubscription");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdDiscount");

                    b.HasIndex("IdSubscriptionOffer");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionOffer", b =>
                {
                    b.Property<int>("IdSubscriptionOffer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSubscriptionOffer");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscriptionOffer"));

                    b.Property<int>("IdRenewalTime")
                        .HasColumnType("int")
                        .HasColumnName("IdRenewalTime");

                    b.Property<int>("IdSoftware")
                        .HasColumnType("int")
                        .HasColumnName("IdSoftware");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("Price");

                    b.HasKey("IdSubscriptionOffer");

                    b.HasIndex("IdRenewalTime");

                    b.HasIndex("IdSoftware");

                    b.ToTable("SubscriptionOffer");
                });

            modelBuilder.Entity("APBD_Projekt.Models.SubscriptionPayment", b =>
                {
                    b.Property<int>("IdSubscriptionPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdSubscriptionPayment");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscriptionPayment"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("DateTime");

                    b.Property<int>("IdSubscription")
                        .HasColumnType("int")
                        .HasColumnName("IdSubscription");

                    b.HasKey("IdSubscriptionPayment");

                    b.HasIndex("IdSubscription");

                    b.ToTable("SubscriptionPayment");
                });

            modelBuilder.Entity("APBD_Projekt.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdUser");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<int>("IdRole")
                        .HasColumnType("int")
                        .HasColumnName("IdRole");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("Password");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("RefreshToken");

                    b.Property<DateTime?>("RefreshTokenExp")
                        .HasColumnType("datetime")
                        .HasColumnName("RefreshTokenExp");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("Salt");

                    b.HasKey("IdUser");

                    b.HasIndex("IdRole");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CategorySoftware", b =>
                {
                    b.Property<int>("CategoriesIdCategory")
                        .HasColumnType("int");

                    b.Property<int>("SoftwaresIdSoftware")
                        .HasColumnType("int");

                    b.HasKey("CategoriesIdCategory", "SoftwaresIdSoftware");

                    b.HasIndex("SoftwaresIdSoftware");

                    b.ToTable("CategorySoftware");
                });

            modelBuilder.Entity("APBD_Projekt.Models.CompanyClient", b =>
                {
                    b.HasBaseType("APBD_Projekt.Models.Client");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("CompanyName");

                    b.Property<string>("KRS")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("KRS");

                    b.ToTable("CompanyClient");
                });

            modelBuilder.Entity("APBD_Projekt.Models.IndividualClient", b =>
                {
                    b.HasBaseType("APBD_Projekt.Models.Client");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LastName");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Name");

                    b.Property<string>("PESEL")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)")
                        .HasColumnName("PESEL");

                    b.ToTable("IndividualClient");
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
                    b.HasOne("APBD_Projekt.Models.RenewalTime", "RenewalTime")
                        .WithMany("SubscriptionOffers")
                        .HasForeignKey("IdRenewalTime")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APBD_Projekt.Models.Software", "Software")
                        .WithMany("SubscriptionOffers")
                        .HasForeignKey("IdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RenewalTime");

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
                        .HasForeignKey("SoftwaresIdSoftware")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("APBD_Projekt.Models.CompanyClient", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Client", null)
                        .WithOne()
                        .HasForeignKey("APBD_Projekt.Models.CompanyClient", "IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("APBD_Projekt.Models.IndividualClient", b =>
                {
                    b.HasOne("APBD_Projekt.Models.Client", null)
                        .WithOne()
                        .HasForeignKey("APBD_Projekt.Models.IndividualClient", "IdClient")
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

            modelBuilder.Entity("APBD_Projekt.Models.RenewalTime", b =>
                {
                    b.Navigation("SubscriptionOffers");
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
