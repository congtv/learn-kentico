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
using System.Collections.Generic;
using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Site1;

[assembly: RegisterDocumentType(LandingPage.CLASS_NAME, typeof(LandingPage))]

namespace CMS.DocumentEngine.Types.Site1
{
	/// <summary>
	/// Represents a content item of type LandingPage.
	/// </summary>
	public partial class LandingPage : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "Site1.LandingPage";


		/// <summary>
		/// The instance of the class that provides extended API for working with LandingPage fields.
		/// </summary>
		private readonly LandingPageFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// Gets an object that provides extended API for working with LandingPage fields.
		/// </summary>
		[RegisterProperty]
		public LandingPageFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with LandingPage fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class LandingPageFields : AbstractHierarchicalObject<LandingPageFields>
		{
			/// <summary>
			/// The content item of type LandingPage that is a target of the extended API.
			/// </summary>
			private readonly LandingPage mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="LandingPageFields" /> class with the specified content item of type LandingPage.
			/// </summary>
			/// <param name="instance">The content item of type LandingPage that is a target of the extended API.</param>
			public LandingPageFields(LandingPage instance)
			{
				mInstance = instance;
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="LandingPage" /> class.
		/// </summary>
		public LandingPage() : base(CLASS_NAME)
		{
			mFields = new LandingPageFields(this);
		}

		#endregion
	}
}