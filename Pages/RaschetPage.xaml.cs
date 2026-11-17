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
        public RaschetPage(string mode)
        {
            _mode = mode;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Clear();
                MainGrid.ColumnDefinitions.Clear();
                int col = -1, row = -1;
                if (string.IsNullOrEmpty(TBColumns.Text) || string.IsNullOrEmpty(TBRows.Text))
                {
                    return;
                }
                if (int.TryParse(TBColumns.Text, out col) && int.TryParse(TBRows.Text, out row))
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
    }
}
