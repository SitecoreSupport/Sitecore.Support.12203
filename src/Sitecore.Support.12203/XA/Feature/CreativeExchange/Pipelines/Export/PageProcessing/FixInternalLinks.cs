using Sitecore.Globalization;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Export.PageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipelines.Export.PageProcessing
{
  public class FixInternalLinks : Sitecore.XA.Feature.CreativeExchange.Pipelines.Export.PageProcessing.FixInternalLinks
  {
    
    protected override void OnLinkProcessed(PageProcessingArgs args, string link, Match match)
    {
      string str = string.Empty;
      if (link.Contains<char>('?'))
      {
        str = link.Substring(link.IndexOf('?'));
        link = link.Substring(0, link.IndexOf('?'));
      }
      #region patch 12203
      Language lan;
      if (link.Length > 1)
      {
        if (link[0] == '/')
        {
          link = link.Substring(1);
        }
        if (link.Contains("/"))
        {
          if (Language.TryParse(link.Substring(0, link.IndexOf('/')), out lan))
          {
            link = link.Substring(link.IndexOf('/'));
          }
        }
        else
        {
          if (Language.TryParse(link, out lan))
          {
            link = "/";
          }
        }
      }
      #endregion
      if (!link.EndsWith("/"))
      {
        link = link + "/";
      }
      link = link + "index.html";
      link = link + str;
      link = args.LinkProcessor.RemoveVirtualDirectory(link, args.PageContext.HomeFullPath, args.PageContext.VirtualFolders);
      link = match.Value.Replace(match.Groups[3].Value, link);
      args.PageHtml = args.PageHtml.Replace(match.Value, link);
    }
  }
}