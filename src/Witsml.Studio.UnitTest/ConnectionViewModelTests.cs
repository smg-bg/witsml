﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.Witsml.Studio.Connections;
using PDS.Witsml.Studio.Properties;
using PDS.Witsml.Studio.Runtime;
using PDS.Witsml.Studio.ViewModels;

namespace PDS.Witsml.Studio
{
    /// <summary>
    /// Unit tests for the ConnectionViewModel
    /// </summary>
    [TestClass]
    public class ConnectionViewModelTests
    {
        private static readonly string PersistedDataFolderName = Settings.Default.PersistedDataFolderName;
        private static readonly string ConnectionBaseFileName = Settings.Default.ConnectionBaseFileName;

        private BootstrapperHarness _bootstrapper;
        private TestRuntimeService _runtime;

        private ConnectionViewModel _witsmlConnectionVm;
        private ConnectionViewModel _etpConnectionVm;
        private Connection _witsmlConnection;
        private Connection _etpConnection;

        /// <summary>
        /// Sets up the environment for each test.  
        /// ConnectionViewModels and Connections are created 
        /// for ConnectionTypes Witsml and Etp.
        /// In addition the persisence folder is cleard and deleted.
        /// </summary>
        [TestInitialize]
        public void TestSetUp()
        {
            _bootstrapper = new BootstrapperHarness();
            _runtime = new TestRuntimeService(_bootstrapper.Container);

            _witsmlConnection = new Connection()
            {
                Name = "Witsml",
                Uri = "http://localhost/Witsml.Web/WitsmlStore.svc",
                Username = "WitsmlUser"
            };

            _etpConnection = new Connection()
            {
                Name = "Etp",
                Uri = "ws://localhost/witsml.web/api/etp",
                Username = "EtpUser"
            };

            _witsmlConnectionVm = new ConnectionViewModel(_runtime, ConnectionTypes.Witsml);
            _etpConnectionVm = new ConnectionViewModel(_runtime, ConnectionTypes.Etp);

            DeletePersistenceFolder();
        }

        /// <summary>
        /// Tests the connection filename for each type of connection has the correct prefix.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilenamePrefix()
        {
            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Witsml.ToString()));

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Etp.ToString()));
        }

        /// <summary>
        /// Tests the connection filename is correctly generated with the 
        /// ConnectionType prefix and the base file name.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilename()
        {
            var witsmlFilename = string.Format("{0}{1}", ConnectionTypes.Witsml, ConnectionBaseFileName);
            var etpFilename = string.Format("{0}{1}", ConnectionTypes.Etp, ConnectionBaseFileName);

            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.AreEqual(witsmlFilename, filename);

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.AreEqual(etpFilename, filename);
        }

        /// <summary>
        /// Tests that a null connection is returned when no connection file is persisted.
        /// </summary>
        [TestMethod]
        public void TestNoConnectionFilePersisted()
        {
            Assert.IsNull(_witsmlConnectionVm.OpenConnectionFile());
            Assert.IsNull(_etpConnectionVm.OpenConnectionFile());
        }

        /// <summary>
        /// Tests the equivalence of a connection before and after file persistence.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilePersistence()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _etpConnectionVm.SaveConnectionFile(_etpConnection);

            var witsmlRead = _witsmlConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_witsmlConnection.Uri, witsmlRead.Uri);

            var etpRead = _etpConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_etpConnection.Uri, etpRead.Uri);
        }

        [TestMethod]
        public void TestInitialeEditItemNoDataItem()
        {
            var emptyConnection = new Connection();
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(emptyConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void TestInitialeEditItemWithDataItem()
        {
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(_witsmlConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void TestInitialeEditItemWithPersistedConnection()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(_witsmlConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public async void TestAcceptWithDataItem()
        {
            var newName = "xxx";

            // Initialze the Edit Item by setting the DataItem
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            // Make a change to the edit item
            _witsmlConnectionVm.EditItem.Name = newName;

            // Accept the changes
            _witsmlConnectionVm.Accept();

            // Test that the DataItem has the new name.
            Assert.AreEqual(newName, _witsmlConnectionVm.DataItem.Name);

            await Task.Yield();
        }

        [TestMethod]
        public void TestCancelWithDataItem()
        {
            var newName = "xxx";

            // Initialze the Edit Item by setting the DataItem
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            // Make a change to the edit item
            _witsmlConnectionVm.EditItem.Name = newName;

            // Accept the changes
            _witsmlConnectionVm.Cancel();

            // Test that the newName was not assigned
            Assert.AreNotEqual(newName, _witsmlConnectionVm.DataItem.Name);

            // Test that the Name is unchanged
            Assert.AreEqual(_witsmlConnection.Name, _witsmlConnectionVm.DataItem.Name);
        }

        private static void DeletePersistenceFolder()
        {
            var path = string.Format("{0}/{1}", Environment.CurrentDirectory, PersistedDataFolderName);

            // Delete the Persistence Folder
            if (Directory.Exists(path))
            {
                // Delete all files in the Persistence Folder
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                Directory.Delete(path);
            }
        }
    }
}