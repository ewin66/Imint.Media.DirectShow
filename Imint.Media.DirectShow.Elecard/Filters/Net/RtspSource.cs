﻿// 
//  RtspSource.cs
//  
//  Author:
//       Simon Mika <simon.mika@imint.se>
//  
//  Copyright (c) 2012-2013 Imint AB
// 
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//  the documentation and/or other materials provided with the distribution.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
using System;
using Error = Kean.Core.Error;
using Kean.Core.Extension;

namespace Imint.Media.DirectShow.Elecard.Filters.Net
{
	public class RtspSource :
	   Abstract
	{
		string url;
		public RtspSource(string url, params DirectShow.Binding.Filters.Abstract[] next) :
			base("net.rtspsource", new System.Guid(global::Elecard.ElUids.Filters.CLSID_RTSPNWSource), "ertspnws.ax", "Elecard RTSP NetSource \"" + url + "\"", next)
		{
			this.url = url;
			this.Output = 0;
			this.OnPreConfigure += configurator =>
			{
				Guid rtspDestination = new Guid(global::Elecard.ElUids.Properties.ERTSP_destination);

				//Console.WriteLine("rtspDestination: " + configurator.GetParamValue(ref rtspDestination, null));

				configurator.SetParamValue(ref rtspDestination, "127.0.0.1");
				configurator.CommitChanges();

				//Console.WriteLine("rtspDestination: " + configurator.GetParamValue(ref rtspDestination, null));
			};
		}
		public override DirectShowLib.IBaseFilter Create()
		{
			DirectShowLib.IBaseFilter result = base.Create();
			if (result is DirectShowLib.IFileSourceFilter)
			{
				Binding.Exception.GraphError.Check((result as DirectShowLib.IFileSourceFilter).Load(this.url, new DirectShowLib.AMMediaType()));
				System.Threading.Thread.Sleep(500);
			}
			return result;
		}
		public override bool Build(DirectShowLib.IPin source, DirectShow.Binding.IBuild build)
		{
			bool result = false;
			DirectShowLib.IBaseFilter filter = this.Create();
			if (build.Graph.AddFilter(filter, "Elecard RTSP NetSource") == 0 && this.PreConfiguration(build))
			{
				foreach (DirectShow.Binding.Filters.Abstract candidate in this.Next)
					if (result = candidate.Build(filter, build))
						break;
			}
			else
			{
				Error.Log.Append(Error.Level.Debug, "Unable to open Elecard RTSP NetSource Filter.", "Elecard RTSP NetSource Filter was unable to open url \"" + this.url + "\".");
				DirectShow.Binding.Exception.GraphError.Check(build.Graph.RemoveFilter(filter));
			}
			return result;
		}
	}
}
