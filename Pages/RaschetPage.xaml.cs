using System;
using System.Collections.Generic;
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

        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExsport_Click(object sender, RoutedEventArgs e)
        {

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
                //Balance();
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
    }
}
