using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WMS.WebUI.Helper
{
    public class ExportExcelhelper
    {
        public static string ExcelContentType
        {
            get
            {
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;
                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);
                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcelDataset(DataSet dataSet, List<string> headings, string sheetHeader, bool showSrNo = false,
           List<string> subHeader = null, List<string> subHeaderForStartColumn = null, params string[] columnsToTake)
        {
            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                for (int datasetIndex = 0; datasetIndex < dataSet.Tables.Count; datasetIndex++)
                {
                    DataTable dataTable = dataSet.Tables[datasetIndex];
                    string heading = headings[datasetIndex];


                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                    int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;
                    workSheet.View.ShowGridLines = false;
                    if (showSrNo)
                    {
                        DataColumn dataColumn = dataTable.Columns.Add("Sr.No.", typeof(string));
                        dataColumn.SetOrdinal(0);
                        int index = 1;
                        for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
                        {
                            DataRow item = dataTable.Rows[i];
                            item[0] = index;
                            index++;
                        }
                    }
                    dataTable.Rows[dataTable.Rows.Count - 1][0] = Convert.ToString("");
                    // add the content into the Excel file  
                    workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                    // autofit width of cells with small content  
                    int columnIndex = 1;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        workSheet.Column(columnIndex).Width = 20;
                        if (column.DataType == typeof(DateTime))
                        {
                            workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex].Style.Numberformat.Format = "dd-MM-yyyy";
                            workSheet.Column(columnIndex).Width = 15;
                        }
                        if (column.DataType == typeof(decimal))
                        {
                            workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex].Style.Numberformat.Format = "#,##0.00";
                        }
                        workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex].Style.WrapText = true;
                        columnIndex++;
                    }
                    workSheet.Column(1).Width = 5;

                    // format header - bold, yellow on black  
                    using (ExcelRange r = workSheet.Cells[workSheet.Dimension.End.Row, 1, workSheet.Dimension.End.Row, dataTable.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }

                    // format header - bold, yellow on black  
                    using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.White);
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    // format cells - add borders  
                    using (ExcelRange r = workSheet.Cells[workSheet.Dimension.Start.Row, 1, workSheet.Dimension.End.Row, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }

                    for (int g = workSheet.Dimension.Start.Row; g < workSheet.Dimension.End.Row; g++)
                    {
                        for (int c = workSheet.Dimension.Start.Column; c < workSheet.Dimension.End.Column; c++)
                        {
                            using (ExcelRange rng = workSheet.Cells[g, c])
                            {
                                if (rng.Value != null && rng.Value.ToString() == "0")
                                    rng.Value = "-";

                            }


                        }
                    }

                    // removed ignored columns  
                    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                    {
                        if (i == 0 && showSrNo)
                        {
                            continue;
                        }
                    }

                    if (subHeader != null)
                    {

                        if (!String.IsNullOrEmpty(subHeader[0]))
                        {
                            workSheet.Cells[subHeaderForStartColumn[0] + 2].Value = subHeader[0];
                            workSheet.Cells[subHeaderForStartColumn[0] + 2].Style.Font.Size = 12;
                            workSheet.Cells[subHeaderForStartColumn[0] + 2].Style.Font.Bold = true;
                            workSheet.Cells[subHeaderForStartColumn[0] + 2].Style.Font.UnderLine = false;
                        }
                    }
                    if (!String.IsNullOrEmpty(sheetHeader))
                    {
                        string decription = heading + " \n " + sheetHeader;
                        string[] details = null;
                        details = decription.Split('\n');
                        int i = 1;
                        foreach (var item in details)
                        {
                            workSheet.Cells["A" + i].Value = item;
                            workSheet.Cells["A" + i].Style.Font.Size = 12;
                            i++;
                        }

                    }
                    else if (!String.IsNullOrEmpty(heading))
                    {
                        workSheet.Cells["A1"].Value = heading;
                        workSheet.Cells["A1"].Style.Font.Size = 12;
                    }
                }

                result = package.GetAsByteArray();

            }

            return result;
        }



        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno);
        }
    }
}
