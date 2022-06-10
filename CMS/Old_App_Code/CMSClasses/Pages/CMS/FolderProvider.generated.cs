//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace CMS.DocumentEngine.Types.CMS
{
	/// <summary>
	/// Provides methods for retrieving pages of type Folder.
	/// </summary>
	public partial class FolderProvider
	{
		/// <summary>
		/// Returns a query that selects published pages of type Folder.
		/// </summary>
		public static DocumentQuery<Folder> GetFolders()
		{
			return DocumentHelper.GetDocuments<Folder>().PublishedVersion().Published();
		}


		/// <summary>
		/// Returns a published page of type Folder that matches the specified criteria.
		/// </summary>
		/// <param name="nodeId">The identifier of the content tree node that represents the page.</param>
		/// <param name="siteName">The name of the site where the page belongs.</param>
		/// <param name="cultureName">The name of the language, e.g. en-US, that determines which localized version should be retrieved.</param>
		public static DocumentQuery<Folder> GetFolder(int nodeId, string cultureName, string siteName)
		{
			return GetFolders().OnSite(siteName).Culture(cultureName).WhereEquals("NodeID", nodeId);
		}


		/// <summary>
		/// Returns a published page of type Folder that matches the specified criteria.
		/// </summary>
		/// <param name="nodeGuid">The globally unique identifier of the content tree node that represents the page.</param>
		/// <param name="siteName">The name of the site where the page belongs.</param>
		/// <param name="cultureName">The name of the language, e.g. en-US, that determines which localized version should be retrieved.</param>
		public static DocumentQuery<Folder> GetFolder(Guid nodeGuid, string cultureName, string siteName)
		{
			return GetFolders().OnSite(siteName).Culture(cultureName).WhereEquals("NodeGUID", nodeGuid);
		}


		/// <summary>
		/// Returns a published page of type Folder that matches the specified criteria.
		/// </summary>
		/// <param name="nodeAliasPath">The alias path to the content tree node that represents the page.</param>
		/// <param name="siteName">The name of the site where the page belongs.</param>
		/// <param name="cultureName">The name of the language, e.g. en-US, that determines which localized version should be retrieved.</param>
		public static DocumentQuery<Folder> GetFolder(string nodeAliasPath, string cultureName, string siteName)
		{
			return GetFolders().OnSite(siteName).Culture(cultureName).Path(nodeAliasPath);
		}
	}
}