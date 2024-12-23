using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{

    IList<Item> Items;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    /*
    public void UpdateQuality()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].Name != "Aged Brie" && Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                if (Items[i].Quality > 0)
                {
                    if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                    {
                        Items[i].Quality = Items[i].Quality - 1;
                    }
                }
            }
            else
            {
                if (Items[i].Quality < 50)
                {
                    Items[i].Quality = Items[i].Quality + 1;

                    if (Items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (Items[i].SellIn < 11)
                        {
                            if (Items[i].Quality < 50)
                            {
                                Items[i].Quality = Items[i].Quality + 1;
                            }
                        }

                        if (Items[i].SellIn < 6)
                        {
                            if (Items[i].Quality < 50)
                            {
                                Items[i].Quality = Items[i].Quality + 1;
                            }
                        }
                    }
                }
            }

            if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
            {
                Items[i].SellIn = Items[i].SellIn - 1;
            }

            if (Items[i].SellIn < 0)
            {
                if (Items[i].Name != "Aged Brie")
                {
                    if (Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (Items[i].Quality > 0)
                        {
                            if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                            {
                                Items[i].Quality = Items[i].Quality - 1;
                            }
                        }
                    }
                    else
                    {
                        Items[i].Quality = Items[i].Quality - Items[i].Quality;
                    }
                }
                else
                {
                    if (Items[i].Quality < 50)
                    {
                        Items[i].Quality = Items[i].Quality + 1;
                    }
                }
            }
        }
    }*/

    public void UpdateQuality()
    {
        foreach (Item item in Items)
        {
            UpdateItem(item);
        }
    }

    public static void UpdateItem(Item item)
    {
        IList<string> itemTypes = GetItemTypes(item);
        int sellInDecrement = GetSellInDecrement(item, itemTypes);
        int qualityDecrement = GetQualityDecrement(item, itemTypes);
        item.SellIn -= sellInDecrement;
        item.Quality -= qualityDecrement;
    }

    public static IList<string> GetItemTypes(Item item)
    {
        IList<string> itemTypes = [];
        if (item.Name == "Aged Brie") itemTypes.Add("AgeBenefitedItem");
        if (item.Name.StartsWith("Backstage passes")) 
        {
            itemTypes.Add("AgeBenefitedItem");
            itemTypes.Add("BackstagePassItem"); 
        }
        if (item.Name.StartsWith("Sulfuras")) itemTypes.Add("LegendaryItem");
        if (item.Name.StartsWith("Conjured")) itemTypes.Add("ConjuredItem");

        return itemTypes;
    }

    public static int GetSellInDecrement(Item item, IList<string> itemTypes)
    {
        return itemTypes.Contains("LegendaryItem") ? 0 : 1;
    }

    public static int GetQualityDecrement(Item item, IList<string> itemTypes)
    {
        int maxQuality = GetMaxQuality(item, itemTypes);
        int qualityDecrement = 1;
        if (itemTypes.Contains("AgeBenefitedItem")) qualityDecrement = -1;
        if (itemTypes.Contains("BackstagePassItem"))
        {
            if (item.SellIn <= 10) qualityDecrement = -2;
            if (item.SellIn <= 5) qualityDecrement = -3;
            if (item.SellIn <= 0) qualityDecrement = 0;
        }
        if (itemTypes.Contains("LegendaryItem") && qualityDecrement > 0) qualityDecrement = 0;
        if (itemTypes.Contains("ConjuredItem")) qualityDecrement *= 2;
        if (item.Quality - qualityDecrement > maxQuality) qualityDecrement = item.Quality - maxQuality;

        return qualityDecrement;
    }

    public static int GetMaxQuality(Item item, IList<string> itemTypes)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return itemTypes.Contains("LegendaryItem") ? 80 : 50;
    }
}