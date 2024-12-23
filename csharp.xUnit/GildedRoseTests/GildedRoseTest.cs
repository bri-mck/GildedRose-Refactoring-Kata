using Xunit;
using System.Collections.Generic;
using GildedRoseKata;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Fact]
    public void GetItemTypesNonSpecialItemNameReturnsEmpty()
    {
        IList<string> itemTypes = GildedRose.GetItemTypes(new Item { Name = "foo", SellIn = 0, Quality = 0 });
        Assert.NotNull(itemTypes);
        Assert.Empty(itemTypes);
    }

    [Fact]
    public void GetItemTypesAgedBrieIsAgeBenefitedItem()
    {
        IList<string> itemTypes = GildedRose.GetItemTypes(new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 });
        Assert.NotNull(itemTypes);
        Assert.Single(itemTypes);
        Assert.Contains(itemTypes, item => item.Contains("AgeBenefitedItem"));
    }

    [Fact]
    public void GetItemTypesBackstagePassIsAgeBenefitedItemAndBackStageItem()
    {
        IList<string> itemTypes = GildedRose.GetItemTypes(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 });
        Assert.NotNull(itemTypes);
        Assert.Equal(2, itemTypes.Count);
        Assert.Contains(itemTypes, item => item.Contains("AgeBenefitedItem"));
        Assert.Contains(itemTypes, item => item.Contains("BackstagePassItem"));
    }

    [Fact]
    public void GetItemTypesSulfurasIsLegendaryItem()
    {
        IList<string> itemTypes = GildedRose.GetItemTypes(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = -1, Quality = 80 });
        Assert.NotNull(itemTypes);
        Assert.Single(itemTypes);
        Assert.Contains(itemTypes, item => item.Contains("LegendaryItem"));
    }

    [Fact]
    public void GetItemTypesIsConjuredItem()
    {
        IList<string> itemTypes = GildedRose.GetItemTypes(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 });
        Assert.NotNull(itemTypes);
        Assert.Single(itemTypes);
        Assert.Contains(itemTypes, item => item.Contains("ConjuredItem"));
    }

    [Fact]
    public void GetSellInDecrementIsZeroForLegendary()
    {
        int sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 0, Quality = 0 }, ["LegendaryItem"]);
        Assert.Equal(0, sellInDecrement);
        sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 0, Quality = 0 }, ["AgeBenefitedItem", "BackstagePassItem", "ConjuredItem", "LegendaryItem"]);
        Assert.Equal(0, sellInDecrement);
    }

    [Fact]
    public void GetSellInDecrementIsOneForNonLegendary()
    {
        int sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, []);
        Assert.Equal(1, sellInDecrement);
        sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, ["AgeBenefitedItem"]);
        Assert.Equal(1, sellInDecrement);
        sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(1, sellInDecrement);
        sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, ["ConjuredItem"]);
        Assert.Equal(1, sellInDecrement);
        sellInDecrement = GildedRose.GetSellInDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, ["AgeBenefitedItem", "BackstagePassItem", "ConjuredItem"]);
        Assert.Equal(1, sellInDecrement);
    }

    [Fact]
    public void GetQualityDecrementForTypicalItemIsOne()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, []);
        Assert.Equal(1, qualityDecrement);
    }

    [Fact]
    public void GetQualityDecrementForAgeBenefitedItemIsNegativeOne()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, ["AgeBenefitedItem"]);
        Assert.Equal(-1, qualityDecrement);
    }

    [Fact]
    public void GetQualityDecrementForBackstageItemIsNegativeTwoWhen5To10DaysOut()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 10, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(-2, qualityDecrement);
        qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 9, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(-2, qualityDecrement);
        qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 6, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(-2, qualityDecrement);
    }

    [Fact]
    public void GetQualityDecrementForBackstageItemIsNegativeThreeWhen5DaysOut()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 5, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(-3, qualityDecrement);
        qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 4, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(-3, qualityDecrement);
        qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 1, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(-3, qualityDecrement);
    }

    [Fact]
    public void GetQualityDecrementForBackstageItemIsZeroWhen0OrLessDaysOut()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 0, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(0, qualityDecrement);
        qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = -1, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(0, qualityDecrement);
    }

    [Fact]
    public void GetQualityDecrementForLegendaryItemIsZero()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["LegendaryItem"]);
        Assert.Equal(0, qualityDecrement);
        qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 100, Quality = 10 }, ["LegendaryItem"]);
        Assert.Equal(0, qualityDecrement);
    }

    [Fact]
    public void GetQualityDecrementForConjuredItemIsDoubled()
    {
        int qualityDecrement = GildedRose.GetQualityDecrement(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["ConjuredItem"]);
        Assert.Equal(2, qualityDecrement);
    }
    
    [Fact]
    public void GetMaxQualityForLegendaryItemIs80()
    {
        int maxQuality = GildedRose.GetMaxQuality(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["LegendaryItem"]);
        Assert.Equal(80, maxQuality);
    }

    [Fact]
    public void GetMaxQualityForNonLegendaryItemIs50()
    {
        int maxQuality = GildedRose.GetMaxQuality(new Item { Name = "foo", SellIn = 11, Quality = 10 }, []);
        Assert.Equal(50, maxQuality);
        maxQuality = GildedRose.GetMaxQuality(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["AgeBenefitedItem"]);
        Assert.Equal(50, maxQuality);
        maxQuality = GildedRose.GetMaxQuality(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["BackstagePassItem"]);
        Assert.Equal(50, maxQuality);
        maxQuality = GildedRose.GetMaxQuality(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["ConjuredItem"]);
        Assert.Equal(50, maxQuality);
        maxQuality = GildedRose.GetMaxQuality(new Item { Name = "foo", SellIn = 11, Quality = 10 }, ["AgeBenefitedItem", "BackstagePassItem", "ConjuredItem"]);
        Assert.Equal(50, maxQuality);
    }

    [Fact]
    public void UpdateItemTypicalItem()
    {
        Item item = new() { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 };
        GildedRose.UpdateItem(item);
        Assert.Equal("+5 Dexterity Vest", item.Name);
        Assert.Equal(9, item.SellIn);
        Assert.Equal(19, item.Quality);
    }

    [Fact]
    public void UpdateItemAgeBenefitedItem()
    {
        Item item = new() { Name = "Aged Brie", SellIn = 2, Quality = 0 };
        GildedRose.UpdateItem(item);
        Assert.Equal("Aged Brie", item.Name);
        Assert.Equal(1, item.SellIn);
        Assert.Equal(1, item.Quality);
    }

    [Fact]
    public void UpdateItemBackstagePassItem()
    {
        Item item = new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 };
        GildedRose.UpdateItem(item);
        Assert.Equal("Backstage passes to a TAFKAL80ETC concert", item.Name);
        Assert.Equal(14, item.SellIn);
        Assert.Equal(21, item.Quality);

        item = new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 };
        GildedRose.UpdateItem(item);
        Assert.Equal("Backstage passes to a TAFKAL80ETC concert", item.Name);
        Assert.Equal(9, item.SellIn);
        Assert.Equal(22, item.Quality);

        item = new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 };
        GildedRose.UpdateItem(item);
        Assert.Equal("Backstage passes to a TAFKAL80ETC concert", item.Name);
        Assert.Equal(4, item.SellIn);
        Assert.Equal(23, item.Quality);
    }

    [Fact]
    public void UpdateItemLegendaryItem()
    {
        Item item = new() { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };
        GildedRose.UpdateItem(item);
        Assert.Equal("Sulfuras, Hand of Ragnaros", item.Name);
        Assert.Equal(0, item.SellIn);
        Assert.Equal(80, item.Quality);
    }

    [Fact]
    public void UpdateItemConjuredItem()
    {
        Item item = new() { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };
        GildedRose.UpdateItem(item);
        Assert.Equal("Conjured Mana Cake", item.Name);
        Assert.Equal(2, item.SellIn);
        Assert.Equal(4, item.Quality);
    }

    [Fact]
    public void UpdateQualityDoesNotChangeItemName()
    {
        IList<Item> Items = [new Item { Name = "foo", SellIn = 0, Quality = 0 }];
        GildedRose app = new(Items);
        app.UpdateQuality();
        Assert.Equal("foo", Items[0].Name);
    }
}