﻿// 
//  Avc.cs
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
using Kean.Core;
using Kean.Core.Extension;


namespace Imint.Media.DirectShow.Elecard.Filters.Decoder.K16
{
	public class Avc :
		Abstract
	{
		public Avc(params DirectShow.Binding.Filters.Abstract[] next) :
			base("decoder.k16.avc", new System.Guid(global::Elecard.ElUids.Filters.CLSID_EAVCDEC_16K), "eavcdec_16k.ax", "Elecard AVC Video Decoder 16K", next)
		{
			this.Output = 0;
		}
		public override DirectShowLib.IBaseFilter Create()
		{
			this.Configure(new string[] { 
				"Software", 
				"Elecard", 
				"Elecard AVC Video Decoder 16K", 
				System.IO.Path.GetFileName(System.Environment.GetCommandLineArgs()[0]) }, 
				KeyValue.Create("Error concealment", 2)
				);
			return base.Create();
		}
	}
}
