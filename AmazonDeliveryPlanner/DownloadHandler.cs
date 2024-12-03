// from https://stackoverflow.com/a/34318792

// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp;
using System;
using System.IO;
using System.Windows.Forms;
// using CefSharp.Example.Handlers;

namespace AmazonDeliveryPlanner
{
    public class DownloadHandler : IDownloadHandler
    {
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return true;
        }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            OnBeforeDownloadFired?.Invoke(this, downloadItem);

            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    string downloadFilePath =
                        string.IsNullOrWhiteSpace(GlobalContext.SerializedConfiguration.DownloadDirectoryPath) ?
                            downloadItem.SuggestedFileName
                            :
                            Path.Combine(GlobalContext.SerializedConfiguration.DownloadDirectoryPath, downloadItem.SuggestedFileName);

                    callback.Continue(downloadFilePath, showDialog: false);

                    // callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
                    GlobalContext.MainWindow.Invoke((MethodInvoker)delegate { System.Media.SystemSounds.Asterisk.Play(); });
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);
        }
    }
}