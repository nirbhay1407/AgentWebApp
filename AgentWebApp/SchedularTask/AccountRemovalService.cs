using OfficeOpenXml;
using Renci.SshNet;
using System.Data;

namespace AgentWebApp.SchedularTask
{
    public class AccountRemovalService : IHostedService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        private Timer? _timer;
        private FileInfo newFile;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AccountRemovalService(IServiceScopeFactory serviceScopeFactory,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {

            _serviceScopeFactory = serviceScopeFactory; _env = env;
        }

        public void Dispose() => _timer?.Dispose();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            /*TimeSpan interval = TimeSpan.FromHours(24);
			//calculate time to run the first time & delay to set the timer
			//DateTime.Today gives time of midnight 00.00
			var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
			var curTime = DateTime.Now;
			var firstInterval = nextRunTime.Subtract(curTime);

			Action action = () =>
			{
				var t1 = Task.Delay(firstInterval);
				t1.Wait();
				//remove inactive accounts at expected time
				//RemoveScheduledAccounts(null);
				//now schedule it to be called every 24 hours for future
				// timer repeates call to RemoveScheduledAccounts every 24 hours.
				_timer = new Timer(
					RemoveScheduledAccounts!,
					null,
					TimeSpan.Zero,
					interval
				);
			};

			// no need to await this call here because this task is scheduled to run much much later.
			Task.Run(action);
			return Task.CompletedTask;*/

            // timer repeates call to RemoveScheduledAccounts every 24 hours.
            _timer = new Timer(
                RemoveScheduledAccounts!,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(2)
            );

            return Task.CompletedTask;


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void RemoveScheduledAccounts(object state)
        {
            /*DataTable dt = new DataTable();
			//Add Datacolumn
			DataColumn workCol = dt.Columns.Add("FirstName", typeof(String));

            dt.Columns.Add("LastName", typeof(String));
            dt.Columns.Add("Blog", typeof(String));
            dt.Columns.Add("City", typeof(String));
            dt.Columns.Add("Country", typeof(String));

            //Add in the datarow
            DataRow newRow = dt.NewRow();

            newRow["firstname"] = "Arun";
            newRow["lastname"] = "Prakash";
            newRow["Blog"] = "http://royalarun.blogspot.com/";
            newRow["city"] = "Coimbatore";
            newRow["country"] = "India";

            dt.Rows.Add(newRow);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			Console.WriteLine("Test");
			using (ExcelPackage pck = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
			{
				//ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Accounts");
				var ws = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Accounts");
				//If worksheet "Content" was not found, add it
				if (ws == null)
				{
					ws = pck.Workbook.Worksheets.Add("Accounts");
				}
				ws.Cells["A1"].LoadFromDataTable(dt, true);
				pck.Save();
			}*/
            //connectToFtpAndOperation();
            //accountServive.DeleteInactiveUserData();
        }


        public void readFile()
        {
            /*ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string[] dirs = Directory.GetFiles(@"C:\Users\nirbhayk\Desktop\FTP\Input", "*.xlsx");
            foreach (string dir in dirs)
            {

                *//*using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                   *//* using (FileStream fs = File.Open(dir, FileMode.Open))
                    {
                        pck.Load(fs);
                    }*//*
                    var ws = pck.Workbook.Worksheets.First();
                }*//*

                FileInfo existingFile = new FileInfo(dir);


                string moveTo = @"C:\Users\nirbhayk\Desktop\FTP\Complete";
                //moving file
                //File.Move(dir, moveTo);


                using (ExcelPackage package = new ExcelPackage())
                {
                    //Load excel stream
                    using (var stream = File.OpenRead(dir))
                    {
                        package.Load(stream);
                    }


                    //get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package?.Workbook.Worksheets.FirstOrDefault();
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count
                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            Console.WriteLine(" Row:" + row + " column:" + col + " Value:" + worksheet.Cells[row, col].Value?.ToString().Trim());
                        }
                    }
                }


                Console.WriteLine(dir);*/


            // }






            /*
                        string ftpUrl = "sftp://nirbhbayasdh.nirbhbayasdhuser@nirbhbayasdh.blob.core.windows.net/Import/"; // Replace with your FTP server URL
                        string userName = "nirbhbayasdhuser"; // Replace with your FTP username
                        string password = "aJIXiXQm3KvY1mqMlhKxz2ZJ3w+Dv7Du"; // Replace with your FTP password

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                        request.Method = WebRequestMethods.Ftp.ListDirectory;
                        request.Credentials = new NetworkCredential(userName, password);

                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    string line = reader.ReadLine();
                                    while (line != null)
                                    {
                                        Console.WriteLine(line);
                                        line = reader.ReadLine();
                                    }
                                }
                            }
                        }*/


            string host = "nirbhbayasdh.blob.core.windows.net";
            string username = "nirbhbayasdh.nirbhbayasdhuser";
            string password = "aJIXiXQm3KvY1mqMlhKxz2ZJ3w+Dv7Du";
            string remoteDirectory = "/Import/";

            // Create an SFTP client
            using (var client = new SftpClient(host, username, password))
            {
                try
                {
                    client.Connect();

                    // Get the list of files in the remote directory
                    var files = client.ListDirectory(remoteDirectory);

                    // Iterate over the files
                    foreach (var file in files)
                    {
                        // Skip directories and parent/relative directories
                        if (file.IsDirectory || file.Name.StartsWith("."))
                            continue;

                        // Download the file
                        using (var stream = new MemoryStream())
                        {
                            client.DownloadFile(Path.Combine(remoteDirectory, file.Name), stream);

                            // Use the file content as needed
                            byte[] fileContent = stream.ToArray();
                            string fileName = file.Name;
                            // Process the file content or save it to disk
                            // ...
                        }
                        client.Delete(file.FullName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    client.Disconnect();
                }
            }

            Console.WriteLine("Done.");
            Console.ReadLine();




        }

        public void saveFileOnSFTP(SftpClient sftpClient, Stream stream, string path)
        {
            sftpClient.UploadFile(stream, path);
        }

        public void GenerateExcel(DataTable Dtvalue)
        {
            /*string excelpath = "";
            excelpath = @"C:\Excels\abc.xlsx";//Server.MapPath("~/UploadExcel/" + DateTime.UtcNow.Date.ToString() + ".xlsx");  
            FileInfo finame = new FileInfo(excelpath);
            if (System.IO.File.Exists(excelpath))
            {
                System.IO.File.Delete(excelpath);
            }
            if (!System.IO.File.Exists(excelpath))
            {
                ExcelPackage excel = new ExcelPackage(finame);
                var sheetcreate = excel.Workbook.Worksheets.Add(DateTime.UtcNow.Date.ToString());
                if (Dtvalue.Rows.Count > 0)
                {
                    for (int i = 0; i < Dtvalue.Rows.Count;)
                    {
                        sheetcreate.Cells[i + 1, 1].Value = Dtvalue.Rows[i][0].ToString();
                        sheetcreate.Cells[i + 1, 2].Value = Dtvalue.Rows[i][1].ToString();
                        sheetcreate.Cells[i + 1, 3].Value = Dtvalue.Rows[i][2].ToString();
                        i++;
                    }
                }
                //sheetcreate.Cells[1, 1, 1, 25].Style.Font.Bold = true;  
                //excel.Save();  

                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                //workSheet.Cells[1, 1].LoadFromCollection(data, true);  
                var filepath = Path.Combine(_env.WebRootPath, "images", "Image1.png");

            }*/
            //return (System.IO.File.ReadAllBytes(workSheet), "image/png", System.IO.Path.GetFileName(filepath));




        }



        public void connectToFtpAndOperation()
        {
            string host = "nirbhbayasdh.blob.core.windows.net";
            string username = "nirbhbayasdh.nirbhbayasdhuser";
            string password = "aJIXiXQm3KvY1mqMlhKxz2ZJ3w+Dv7Du";
            string remoteDirectory = "/Import/";
            string CompletedDirectory = "/Completed/";
            string ErrorDirectory = "/Exception/";
            int port = 22;

            DataTable result = ReadExcelFilesFromSftp(host, port, username, password, remoteDirectory, CompletedDirectory, ErrorDirectory);
        }

        private DataTable ReadExcelFilesFromSftp(string host, int port, string username, string password, string directory, string completedDirectory, string ErrorDirectory)
        {
            DataTable dataTable = new DataTable();
            DataTable dataTableError = new DataTable();

            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();

                var files = client.ListDirectory(directory);

                foreach (var file in files)
                {
                    dataTable = new DataTable();
                    dataTableError = new DataTable();
                    if (!file.IsDirectory)
                    {
                        using (var stream = new MemoryStream())
                        {
                            client.DownloadFile(file.FullName, stream);
                            stream.Position = 0;

                            using (var package = new ExcelPackage(stream))
                            {
                                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                                bool hasHeader = true;

                                foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                                {
                                    dataTable.Columns.Add(hasHeader ? firstRowCell.Text : $"Column{firstRowCell.Start.Column}");
                                    dataTableError.Columns.Add(hasHeader ? firstRowCell.Text : $"Column{firstRowCell.Start.Column}");
                                }
                                dataTable.Columns.Add("ErrorReason");
                                dataTableError.Columns.Add("ErrorReason");

                                var startRow = hasHeader ? 2 : 1;
                                int counter = 0;
                                for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                                {
                                    DataRow dataRow;
                                    DataRow dataRowErr;
                                    var worksheetRow = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
                                    dataRow = dataTable.Rows.Add();
                                    dataRowErr = dataTableError.Rows.Add();

                                    foreach (var cell in worksheetRow)
                                    {
                                        dataRow[cell.Start.Column - 1] = cell.Text;
                                        dataRowErr[cell.Start.Column - 1] = cell.Text;
                                    }
                                    dataRow = ValidateEmpList(dataRow);
                                    dataRowErr = dataRow;
                                    if (dataRow["ErrorReason"] is DBNull)
                                    {
                                        SaveDataRowEmployee(dataRow);
                                        dataTableError.Rows.Remove(dataRowErr);
                                    }
                                    else
                                        dataTable.Rows.Remove(dataRow);
                                    counter++;
                                }
                            }
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            dataTable.Columns.Remove("ErrorReason");
                            ConvertDataTableToExcelAndUpload(dataTable, client, completedDirectory, file.Name);
                        }
                        if (dataTableError.Rows.Count > 0)
                            ConvertDataTableToExcelAndUpload(dataTableError, client, ErrorDirectory, file.Name);
                    }
                    //client.Delete(file.FullName);
                }

                client.Disconnect();
            }

            return dataTable;
        }

        public DataRow ValidateEmpList(DataRow row)
        {
            if (row["FirstName"] is not DBNull && row["LastName"] is not DBNull && row[3] is not DBNull && row[4] is not DBNull)
            {
                var branchByGpBranch = "09";//await commonService.GetBranchDetailsByGPBranch(row["LocationName);
                if (branchByGpBranch == null)
                {
                    row["ErrorReason"] = "GPBranchLocation is not available in database";
                }
                var stateData = "aa";//employeesService.GetStateById(row["State).Result;
                if (stateData == null)
                {
                    if (row["ErrorReason"] is DBNull)
                        row["ErrorReason"] += "State is not mapped in database";
                    else
                        row["ErrorReason"] += ", " + "State is not mapped in database";
                }

                if (row["Country"]?.ToString()?.ToUpper() == "US" || row["Country"]?.ToString()?.ToUpper() == "UNITED STATES" || row["Country"]?.ToString()?.ToUpper() == "CANADA") { }
                else
                {
                    if (row["ErrorReason"] is DBNull)
                        row["ErrorReason"] = "Country is not Valid";
                    else
                        row["ErrorReason"] += ", " + "Country is not Valid";
                }
                if ((row["EmployeeNumber"] is DBNull ? 0 : Convert.ToInt32(row["EmployeeNumber"])) == 0)
                {
                    if (row["ErrorReason"] is DBNull)
                        row["ErrorReason"] = "GP EmployeeID is Required";
                    else
                        row["ErrorReason"] += ", " + "GP EmployeeID is Required";
                }

                if (row["AddressLine1"] is DBNull)
                {
                    if (row["ErrorReason"] is DBNull)
                        row["ErrorReason"] = "Address Line1  is Required";
                    else
                        row["ErrorReason"] += ", " + "Address Line1  is Required";
                }

                if (row["Contact1"] is DBNull || row["Contact1"]?.ToString()?.Length != 10)
                {
                    if (row["ErrorReason"] is not DBNull)
                        row["ErrorReason"] = row["Contact1"] == null ? "Contact1 is Required." : "Contanct1 is not valid.";
                    else
                        row["ErrorReason"] += ", " + row["Contact1"] == null ? "Contact1 is Required." : "Contanct1 is not valid.";
                }
                else
                    row["Contact1"] = String.Format("{0:(###) ###-####}", Convert.ToInt64(row["Contact1"]));
                if (row["Contact2"] != null && row["Contact2"]?.ToString()?.Length == 10)
                    row["Contact2"] = String.Format("{0:(###) ###-####}", Convert.ToInt64(row["Contact2"]));
            }
            return row;
        }


        private void ConvertDataTableToExcelAndUpload(DataTable dataTable, SftpClient client, string remoteDirectory, string fileName)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                }

                // Write DataTable data to the Excel worksheet
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    //if(row["ErrorReason"])
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {

                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Save the Excel package to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    stream.Position = 0;

                    // Upload the MemoryStream to the SFTP server


                    if (!client.Exists(remoteDirectory))
                    {
                        client.CreateDirectory(remoteDirectory);
                    }

                    client.ChangeDirectory(remoteDirectory);
                    client.UploadFile(stream, fileName);
                }
            }
        }

        private bool SaveDataRowEmployee(DataRow dtRow)
        {
            /*foreach (DataRow dtRow in dt.Rows)
            {*/
            //EmployeeImportDataModel obj = new EmployeeImportDataModel();
            var FirstName = dtRow["FirstName"] is DBNull ? null : dtRow["FirstName"].ToString();
            var LastName = dtRow["LastName"] is DBNull ? null : dtRow["LastName"].ToString();
            var GPEmployeeID = dtRow["EmployeeNumber"] is DBNull ? 0 : Convert.ToInt32(dtRow["EmployeeNumber"]);
            var DOB = dtRow["DOB"] is DBNull ? null : dtRow["DOB"].ToString();
            var HireDate = dtRow["HireDate"] is DBNull ? null : dtRow["HireDate"].ToString();
            var AdddressLine1 = dtRow["AddressLine1"] is DBNull ? null : dtRow["AddressLine1"].ToString();
            var AdddressLine2 = dtRow["AddressLine2"] is DBNull ? null : dtRow["AddressLine2"].ToString();
            var AdddressLine3 = dtRow["AddressLine3"] is DBNull ? null : dtRow["AddressLine3"].ToString();
            var City = dtRow["City"] is DBNull ? null : dtRow["City"].ToString();
            var State = dtRow["State"] is DBNull ? null : dtRow["State"].ToString();
            var ZipCode = dtRow["ZipCode"] is DBNull ? null : dtRow["ZipCode"].ToString();
            var Contact1 = dtRow["Contact1"] is DBNull ? null : dtRow["Contact1"].ToString();
            var Contact2 = dtRow["Contact2"] is DBNull ? null : dtRow["Contact2"].ToString();
            var Email = dtRow["Email"] is DBNull ? null : dtRow["Email"].ToString();
            var Country = dtRow["Country"] is DBNull ? null : dtRow["Country"].ToString();
            var LocationName = dtRow["LocationName"] is DBNull ? null : dtRow["LocationName"].ToString();

            /* }*/


            return true;
        }


        public DataTable ConvertToDatatable(MemoryStream stream)
        {
            DataTable dt = new DataTable();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                pck.Load(stream);
                var ws = pck.Workbook.Worksheets.First();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    // dt.Columns.Add(new DataColumn(firstRowCell.Text));
                    dt.Columns.Add(true ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                dt.Columns.Add(new DataColumn("ErrorReason"));
                if (dt.Columns[0].ToString() == "FirstName")// && secondCell == "LastName" && thirdCell == "EmployeeNumber" && fourthCell == "DOB")
                {
                    var startRow = true ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = dt.Rows.Add();
                        row[0] = rowNum;
                        row[1] = rowNum;

                        dt = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column].ToDataTable(c =>
                        {
                            c.FirstRowIsColumnNames = true;
                        });
                    }
                }

            }

            return dt;
        }
    }
}
