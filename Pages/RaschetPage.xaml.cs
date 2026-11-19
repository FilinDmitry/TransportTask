using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TransportTask.Pages
{
    /// <summary>
    /// Логика взаимодействия для RaschetPage.xaml
    /// </summary>
    public partial class RaschetPage : Page
    {
        private string _mode;
        private int col = -1, row = -1;
        public RaschetPage(string mode)
        {
            _mode = mode;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateField(TBColumns.Text, TBRows.Text);
        }

        private void ButtonBalanse_Click(object sender, RoutedEventArgs e)
        {
            Balance();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void ButtonRaschet_Click(object sender, RoutedEventArgs e)
        {
            switch (_mode)
            {
                case "СЗ":
                    NorthWest();
                    break;
                case "Мин":
                    MinElements();
                    break;
            }
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                FileName = "Transport.xlsx"
            };
            if (dlg.ShowDialog() == true)
            {
                ImportFromExcel(dlg.FileName);
            }
        }

        private void ButtonExsport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                FileName = "Transport.xlsx"
            };
            if (dlg.ShowDialog() == true)
            {
                ExportToExcel(dlg.FileName);
            }
        }


        private void TB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        private void CreateField(string str_col, string str_row)
        {
            try
            {
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Clear();
                MainGrid.ColumnDefinitions.Clear();
                
                if (string.IsNullOrEmpty(str_row) || string.IsNullOrEmpty(str_col))
                {
                    return;
                }
                if (int.TryParse(str_col, out col) && int.TryParse(str_row, out row))
                {
                    if (row > 0 && col > 0)
                    {
                        if (row > 20 && col > 20)
                        {
                            MessageBox.Show("Слишком много колонок/столбцов");
                            return;
                        }
                        for (int i = 0; i < row + 2; i++)
                        {
                            MainGrid.RowDefinitions.Add(new RowDefinition());

                        }
                        for (int i = 0; i < col + 2; i++)
                        {
                            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        }
                        for (int i = 1; i < row + 2; i++)
                        {

                            for (int j = 1; j < col + 2; j++)
                            {

                                TextBox tb = new TextBox();
                                tb.Width = 50;
                                tb.PreviewTextInput += TB_PreviewTextInput;
                                Grid.SetColumn(tb, j);
                                Grid.SetRow(tb, i);
                                if (i == row + 1 && j == col + 1) break;
                                if (i == row + 1)
                                {
                                    tb.Background = new SolidColorBrush(Colors.LightGray);
                                }
                                if (j == col + 1)
                                {
                                    tb.Background = new SolidColorBrush(Colors.LightGray);
                                }

                                MainGrid.Children.Add(tb);
                            }
                        }


                    }
                }
                else
                {
                    TBColumns.Text = "";
                    TBRows.Text = "";
                }
            }
            catch
            {

            }
        }
        
        private void Balance()
        {
            try
            {
                int sum_cols = MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetRow(tb) == row + 1).Sum(tb => int.Parse(tb.Text));
                int sum_rows = MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetColumn(tb) == col + 1).Sum(tb => int.Parse(tb.Text));
                if (sum_rows == sum_cols)
                {
                    MessageBox.Show("Матрица сбалансирована");
                    return;
                }

                if (sum_cols > sum_rows)
                {
                    AddRow(sum_cols - sum_rows);

                }
                else
                {
                    AddColumn(sum_rows - sum_cols);
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }

        }
        private void AddRow(int def)
        {
            int target = row + 1;
            MainGrid.RowDefinitions.Add(new RowDefinition());
            foreach (TextBox tb in MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetRow(tb) == target).ToList())
            {
                Grid.SetRow(tb, target + 1);
            }

            for (int j = 1; j < col + 2; j++)
            {
                var newTb = new TextBox();
                newTb.PreviewTextInput += TB_PreviewTextInput;
                newTb.Width = 50;
                if (j == col + 1)
                {
                    newTb.Text = def.ToString();
                    newTb.Background = new SolidColorBrush(Colors.LightGray);
                }

                Grid.SetColumn(newTb, j);
                Grid.SetRow(newTb, target);
                MainGrid.Children.Add(newTb);
            }
            row += 1;
            

        }

        private void AddColumn(int def)
        {
            int target = col + 1;
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (TextBox tb in MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetColumn(tb) == target).ToList())
            {
                Grid.SetColumn(tb, target + 1);
            }

            for (int i = 1; i < row + 2; i++)
            {
                var newTb = new TextBox();
                newTb.Width = 50;
                newTb.PreviewTextInput += TB_PreviewTextInput;
                

                if (i == row + 1)
                {
                    newTb.Text = def.ToString();
                    newTb.Background = new SolidColorBrush(Colors.LightGray);
                }

                Grid.SetRow(newTb, i);
                Grid.SetColumn(newTb, target);
                MainGrid.Children.Add(newTb);
            }
            col += 1;
        }
        private void Clear()
        {
            foreach(TextBox tb in MainGrid.Children.OfType<TextBox>().ToList())
            {
                tb.Text = "";
            }
        }
        private void ImportFromExcel(string path)
        {
            try
            {

                using (var p = new ExcelPackage(path))
                {

                    var ws = p.Workbook.Worksheets[0];

                    if (ws.Dimension == null)
                    {
                        MessageBox.Show("Файл пустой");
                        return;
                    }
                    int newMaxRow = ws.Dimension.End.Row - 1;
                    int newMaxCol = ws.Dimension.End.Column - 1;
                    if (newMaxRow < 2 || newMaxCol < 2)
                    {
                        MessageBox.Show("Данных мало");
                        return;
                    }
                    if (newMaxCol > 21 || newMaxRow > 21)
                    {
                        MessageBox.Show("Файл слишком большой");
                    }
                    TBRows.Text = newMaxRow.ToString();
                    TBColumns.Text = newMaxCol.ToString();

                    for (int i = 1; i <= newMaxRow + 1; i++)
                    {
                        for (int j = 1; j <= newMaxCol + 1; j++)
                        {
                            if (i == newMaxRow + 1 && j == newMaxCol + 1)
                            { continue; }

                            var tb = GetCell(i, j);
                            if (tb != null)
                                tb.Text = ws.Cells[i, j].Text ?? string.Empty;
                        }
                    }

                   
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }



        private void ExportToExcel(string path)
        {
            try
            {
                int totalRows = row + 1;
                int totalCols = col + 1;
                
                using (var p = new ExcelPackage())
                {

                    var ws = p.Workbook.Worksheets.Add("Result");
                    for (int i = 1; i <= row + 1; i++)
                    {
                        for (int j = 1; j <= col + 1; j++)
                        {
                            if (i == row + 1 && j == col + 1)
                            {
                                continue;
                            }
                            var tb = GetCell(i, j);
                            ws.Cells[i, j].Value = tb?.Text ?? string.Empty;
                        }
                    }
                    p.SaveAs(path);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }
            }
            catch
            {
                MessageBox.Show("Что-то ошибка");
            }
        }
        private TextBox GetCell(int i, int j)
        {
            return MainGrid.Children.OfType<TextBox>().First(tb => Grid.GetRow(tb) == i && Grid.GetColumn(tb) == j);
        }

        private void MinElements()
        {
            int sum_cols = MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetRow(tb) == row + 1).Sum(tb => int.Parse(tb.Text));
            int sum_rows = MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetColumn(tb) == col + 1).Sum(tb => int.Parse(tb.Text));
            if (sum_rows != sum_cols)
            {
                MessageBox.Show("Матрица НЕсбалансирована");
                return;
            }
        }

        private void NorthWest()
        {
            try
            {
                List<TextBox> sum_cols = MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetRow(tb) == row + 1).OrderByDescending(Grid.GetColumn).ToList();
                List<TextBox> sum_rows = MainGrid.Children.OfType<TextBox>().Where(tb => Grid.GetColumn(tb) == col + 1).OrderByDescending(Grid.GetRow).ToList();
                if (sum_rows.Sum(tb => int.Parse(tb.Text)) != sum_cols.Sum(tb => int.Parse(tb.Text)))
                {
                    MessageBox.Show("Матрица НЕсбалансирована");
                    return;
                }
                Stack<int> stack_col = new Stack<int>();
                Stack<int> stack_row = new Stack<int>();
                
                foreach (TextBox tb in sum_cols)
                {
                    stack_col.Push(int.Parse(tb.Text));
                }
                foreach (TextBox tb in sum_rows)
                {
                    stack_row.Push(int.Parse(tb.Text));
                }
                foreach (var tb in MainGrid.Children.OfType<TextBox>())
                {
                    tb.Text = string.Empty;
                }
                NorthWest(stack_col, stack_row);
            }
            catch (Exception e)
            {
                
                MessageBox.Show("Ошибка выполнения расчетов");
            }
        }
        private void NorthWest(Stack<int> columns, Stack<int> rows)
        {

            if (columns.Count == 0 || rows.Count == 0)
            {
                return;
            }
            
            TextBox textBox = GetCell(row + 1 - rows.Count, col + 1 - columns.Count);
            int cur_col = columns.Peek();
            int cur_row = rows.Peek();
            columns.Pop();
            rows.Pop();
            if (cur_col <= cur_row)
            {
                rows.Push(cur_row - cur_col);
                textBox.Text = cur_col.ToString();
            }
            else if (cur_col >= cur_row)
            {
                columns.Push(cur_col - cur_row);
                textBox.Text = cur_row.ToString();
            }
            else
            {
                textBox.Text = cur_col.ToString();
            }

            NorthWest(columns, rows);
        }
    }
}
