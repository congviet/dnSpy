﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.ILSpy.AvalonEdit;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using dnlib.DotNet;

namespace ICSharpCode.ILSpy.Bookmarks
{
	/// <summary>
	/// A bookmark that can be attached to an AvalonEdit TextDocument.
	/// </summary>
	public class BookmarkBase : IBookmark
	{
		TextLocation location;
		TextLocation endLocation;

		protected void Modified()
		{
			if (OnModified != null)
				OnModified(this, EventArgs.Empty);
		}
		public event EventHandler OnModified;
		
		public TextLocation Location {
			get { return location; }
			set {
				if (location != value) {
					location = value;
					Modified();
				}
			}
		}
		
		public TextLocation EndLocation {
			get { return endLocation; }
			set {
				if (endLocation != value) {
					endLocation = value;
					Modified();
				}
			}
		}
		
		protected virtual void Redraw()
		{
		}
		
		public IMemberRef MemberReference { get; private set; }
		
		public int LineNumber {
			get { return location.Line; }
		}
		
		public virtual int ZOrder {
			get { return 0; }
		}

		/// <summary>
		/// true if the bookmark is visible in the displayed document
		/// </summary>
		public virtual bool IsVisible {
			get { return true; }
		}
		
		/// <summary>
		/// Gets if the bookmark can be toggled off using the 'set/unset bookmark' command.
		/// </summary>
		public virtual bool CanToggle {
			get {
				return true;
			}
		}
		
		public BookmarkBase(IMemberRef member, TextLocation location, TextLocation endLocation)
		{
			this.MemberReference = member;
			this.Location = location;
			this.EndLocation = endLocation;
		}
		
		public virtual ImageSource Image {
			get { return null; }
		}
		
		public virtual void MouseDown(MouseButtonEventArgs e)
		{
		}
		
		public virtual void MouseUp(MouseButtonEventArgs e)
		{
		}

		protected ITextMarker CreateMarkerInternal(ITextMarkerService markerService)
		{
			var line = markerService.TextView.Document.GetLineByNumber(location.Line);
			var endLine = markerService.TextView.Document.GetLineByNumber(endLocation.Line);
			int startOffset = line.Offset + location.Column - 1;
			int endOffset = endLine.Offset + endLocation.Column - 1;

			return markerService.Create(startOffset, endOffset - startOffset);
		}

		public void UpdateLocation(TextLocation location, TextLocation endLocation)
		{
			if (this.location == location && this.endLocation == endLocation)
				return;
			this.location = location;
			this.endLocation = endLocation;
			Modified();
		}
	}
}