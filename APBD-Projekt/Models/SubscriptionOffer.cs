﻿namespace APBD_Projekt.Models;

public class SubscriptionOffer
{
    public int IdSubscriptionOffer { get; private set; }
    public string Name { get; private set; }
    public int IdSoftware { get; private set; }
    public decimal Price { get; private set; }
    public int IdRenewalTime { get; private set; }

    public Software Software { get; private set; }
    public RenewalTime RenewalTime { get; private set; }
    public ICollection<Subscription> Subscriptions { get; private set; }

    protected SubscriptionOffer()
    {
    }

    public SubscriptionOffer(string name, decimal price, Software software, RenewalTime renewalTime)
    {
        Name = name;
        Price = price;
        Software = software;
        RenewalTime = renewalTime;
    }
}