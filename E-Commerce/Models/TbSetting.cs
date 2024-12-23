using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class TbSetting
{
    public int Id { get; set; }

    public string WebsiteName { get; set; } = null!;

    public string WebsiteDescription { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string FacebookLink { get; set; } = null!;

    public string TwiterLink { get; set; } = null!;

    public string InstgramLink { get; set; } = null!;

    public string YoutubeLink { get; set; } = null!;

    public string GoogleLink { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string CallUs { get; set; } = null!;

    public string EmailWebsite { get; set; } = null!;

    public string Fax { get; set; } = null!;

    public string ImageWebsite { get; set; } = null!;

    public string MessageWelcom { get; set; } = null!;

    public string WebsiteDescriptionCenter { get; set; } = null!;

    public string MainName1 { get; set; } = null!;

    public string MainName2 { get; set; } = null!;

    public string MainName3 { get; set; } = null!;

    public string MainName4 { get; set; } = null!;

    public string FooterWelcom { get; set; } = null!;

    public string FooterDescription { get; set; } = null!;
}
