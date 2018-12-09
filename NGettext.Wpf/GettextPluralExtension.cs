using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;
namespace NGettext.Wpf
{
	public class GettextPluralExtension :  MarkupExtension, IWeakCultureObserver
	{
		private DependencyObject _dependencyObject;
		private DependencyProperty _dependencyProperty;

		[ConstructorArgument("params")] public object[] Params { get; set; }

		[ConstructorArgument("singleMsgId")] public string SingleMsgId { get; set; }
		[ConstructorArgument("pluralMsgId")] public string PluralMsgId { get; set; }
		/// <summary>
		/// Defines if the singular or plural should be used
		/// </summary>
		[ConstructorArgument("N")] public int N { get; set; }

		public GettextPluralExtension(string singleMsgId) : this(singleMsgId, null, 1) { }


		public GettextPluralExtension(string singleMsgId, string pluralMsgId, string N) : this(singleMsgId, pluralMsgId, int.Parse(N), new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, byte N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, sbyte N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, short N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, ushort N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		//public GettextPluralExtension(string singleMsgId, string pluralMsgId, Int32 N ) : this(singleMsgId, pluralMsgId, (int)N,   new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, uint N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, long N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, ulong N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, double N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, float N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, decimal N) : this(singleMsgId, pluralMsgId, (int)N, new object[] { }) { }


		public GettextPluralExtension(string singleMsgId, string pluralMsgId, byte N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, sbyte N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, short N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, ushort N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		//public GettextPluralExtension(string singleMsgId, string pluralMsgId, Int32 N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, uint N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, long N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, ulong N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, double N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, float N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }
		public GettextPluralExtension(string singleMsgId, string pluralMsgId, decimal N, params object[] @params) : this(singleMsgId, pluralMsgId, (int)N, @params) { }





		public GettextPluralExtension(string singleMsgId, string pluralMsgId, int N, params object[] @params)
		{
			SingleMsgId = singleMsgId;
			PluralMsgId = pluralMsgId;
			this.N = N;
			Params = @params;
		}

		public static ILocalizer Localizer { get; set; }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			IProvideValueTarget provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			_dependencyObject = (DependencyObject)provideValueTarget.TargetObject;
			if (DesignerProperties.GetIsInDesignMode(_dependencyObject))
			{
				return string.Format(SingleMsgId, Params);
			}

			AttachToCultureChangedEvent();

			_dependencyProperty = (DependencyProperty)provideValueTarget.TargetProperty;

			KeepGettextExtensionAliveForAsLongAsDependencyObject();

			return Gettext();
		}

		private string Gettext()
		{


			return Translation.PluralGettext(N, SingleMsgId, PluralMsgId, Params);
		}

		private void KeepGettextExtensionAliveForAsLongAsDependencyObject()
		{
			SetGettextExtension(_dependencyObject, this);
		}

		private void AttachToCultureChangedEvent()
		{
			if (Localizer is null)
			{
				Console.Error.WriteLine("NGettext.WPF.GettextExtension.Localizer not set.  Localization is disabled.");
				return;
			}

			Localizer.CultureTracker.AddWeakCultureObserver(this);
		}

		public void HandleCultureChanged(ICultureTracker sender, CultureEventArgs eventArgs)
		{
			_dependencyObject.SetValue(_dependencyProperty, Gettext());
		}

		public static readonly DependencyProperty GettextExtensionProperty = DependencyProperty.RegisterAttached(
			"GettextExtension", typeof(GettextPluralExtension), typeof(GettextPluralExtension), new PropertyMetadata(default(GettextPluralExtension)));

		public static void SetGettextExtension(DependencyObject element, GettextPluralExtension value)
		{
			element.SetValue(GettextExtensionProperty, value);
		}

		public static GettextPluralExtension GetGettextExtension(DependencyObject element)
		{
			return (GettextPluralExtension)element.GetValue(GettextExtensionProperty);
		}
	}
}