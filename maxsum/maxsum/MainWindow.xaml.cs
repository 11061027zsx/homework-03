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
using System.Data;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;

using System.Media;
using System.Xml;
using System.Windows.Markup;

namespace maxsum
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 




    public partial class MainWindow : Window
    {
        public ObservableCollection<items> CollectionSource { get; set; }

        private static ListBoxItem addnewitem = new ListBoxItem();

        List<TabItemDetail> tabitem = new List<TabItemDetail>();

        public MainWindow()
        {
            InitializeComponent();
            init();
        }





        //处理UI和数据的几个方法
        
        
        //初始化
        void init()
        {
            dataGrid.Visibility = Visibility.Collapsed;
            setdata();
            setui();
        }


        //前台UI数据
        void setdata()
        {
            addnewitem.Content = "+";
            addnewitem.HorizontalAlignment = HorizontalAlignment.Center;
            TabItemDetail x = new TabItemDetail() { IsArrResourceGet = false,n=0,m=0 };
            tabitem.Add(x);
        }


        //前台UI控件
        void setui()
        {
            int n, m, x,_result=0;
            x = UITabMain.SelectedIndex;
            if (x < 0) return;
            if (!tabitem[x].IsArrResourceGet)
            {
                showaddnewfile();
                return;
            }

            n = tabitem[x].n;
            m = tabitem[x].m;


            UICpuLoad.Text = tabitem[x].Acpuload.ToString();
            UIfilename.Text = tabitem[x].filename;

            UIv.IsChecked = tabitem[x].IsVSelect;
            UIh.IsChecked = tabitem[x].IsHSelect;
            UIa.IsChecked = tabitem[x].IsASelect;


            bool[,] tt = new bool[100, 100];
            int mm=m, nn=n;
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 50; j++) tt[i, j] = false;

            if (!tabitem[x].IsASelect)
            {
                if (tabitem[x].IsHSelect && tabitem[x].IsVSelect)
                {
                    tt = tabitem[x].VHg;
                    nn = n * 2;
                    mm = m * 2;
                    _result = tabitem[x].VHmaxsum;

                }
                else if (!tabitem[x].IsHSelect && tabitem[x].IsVSelect)
                {
                    tt = tabitem[x].Vg;
                    mm = m * 2;
                    _result = tabitem[x].Vmaxsum;
                }
                else if (tabitem[x].IsHSelect && !tabitem[x].IsVSelect)
                {
                    tt = tabitem[x].Hg;
                    nn = n * 2;
                    _result = tabitem[x].Hmaxsum;
                }
                else if (!tabitem[x].IsHSelect && !tabitem[x].IsVSelect)
                {
                    tt = tabitem[x].g;
                    _result = tabitem[x].maxsum;
                }
            }
            else
            {

                /// any G
                mm = m;
                nn = n;




                // NONE VH
                if (!tabitem[x].IsVSelect && !tabitem[x].IsHSelect)
                {
                    tt = tabitem[x].Ag;
                    _result = tabitem[x].Amaxsum;
                }

                // V
                else if (tabitem[x].IsVSelect && !tabitem[x].IsHSelect)
                {
                    tt = tabitem[x].AVg;
                    _result = tabitem[x].AVmaxsum;
                }

                // H
                else if (!tabitem[x].IsVSelect && tabitem[x].IsHSelect)
                {
                    tt = tabitem[x].AHg;
                    _result = tabitem[x].AHmaxsum;
                }

                ///VH
                else if (tabitem[x].IsVSelect && tabitem[x].IsHSelect)
                {
                    tt = tabitem[x].AVHg;
                    _result = tabitem[x].AVHmaxsum;
                }
            }


            CollectionSource = new ObservableCollection<items>();
            CollectionSource.Clear();

            for (int i = 0; i < mm; i++)
            {
                items dr = new items();
                dr.Clear();
                for (int j = 0; j < nn; j++)
                {

                    item ttt = new item { Back = "White", Value = tabitem[x].ArrResource[i%m, j%n] };
                    dr.Add(ttt);
                    if (tt[i, j]) dr[j].Back = "LightSkyBlue";

                }
                CollectionSource.Add(dr);
            }

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = CollectionSource;


            showdatagrid();

            UIMaxsumResult.Text = _result.ToString();

        }


        //关联ss对应路径的文件，并提取相关的数据
        void getdata(string ss)
        {
            string s = "";
            int[,] g, f, l, a,p;
            int i, j, k,y, nx = 0;

            int x = UITabMain.SelectedIndex;

            var file = File.OpenText(ss);
            
            s = file.ReadToEnd();
            s = s.Replace("\r\n", " ");

            string[] sa = s.Split(' ',',',(char)9);

            int kk = 0,m=0,n=0;

            while (!int.TryParse(sa[kk], out m)) kk++;
            kk++;

            while (!int.TryParse(sa[kk], out n)) kk++;
            kk++;

            int mm = m * 2, nn = 2 * n;

            int[,] aa=new int[m,n];

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    int tt = 0;
                    if (kk >= sa.Length)
                    {
                        MessageBox.Show("此文件数据格式错误或数据数量错误");
                        return;
                    }
                    while (!int.TryParse(sa[kk], out tt)) kk++;
                    kk++;
                    aa[i, j] = tt;
                }





            tabitem[x] = new TabItemDetail()
            {
                m = m
                ,
                n = n
                ,
                ArrResource = new int[m, n]
                ,
                IsArrResourceGet = true
                ,IsASelect=false
                ,IsHSelect=false
                ,IsVSelect=false
                ,Vg=new bool[mm,nn]
                ,VHg=new bool[mm,nn]
                ,Hg=new bool[mm,nn]
                ,g=new bool[mm,nn]
                ,Ag=new bool[m,n]
                ,AVg=new bool[m,n]
                ,AVHg=new bool[m,n]
                ,AHg=new bool[m,n]

            };



            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    tabitem[x].ArrResource[i, j] = aa[i, j];
                }

            




            g = new int[mm + nn, mm + nn];
            f = new int[mm + nn, mm + nn];
            a = new int[mm + nn, mm + nn];
            l = new int[mm + nn, mm + nn];
            p = new int[mm + nn, mm + nn];

            for (i = 0; i < mm; i++)
                for (j = 0; j < nn; j++)
                {
                    a[i, j] = tabitem[x].ArrResource[i % m, j % n];
                    g[i, j] = 0;
                }

            

            for (i = 0; i < mm; i++)
            {
                g[i, 0] = a[i, 0];
                for (j = 1; j < nn; j++) g[i, j] = g[i, j - 1] + a[i, j];
            }


            maxsumresult v = new maxsumresult { value = a[0, 0], len = 1, x = 0, s = 0, y = 0 }, vh = new maxsumresult { value = a[0, 0], len = 1, x = 0, s = 0, y = 0 };
            maxsumresult h = new maxsumresult { value = a[0, 0], len = 1, x = 0, s = 0, y = 0 }, nonevh = new maxsumresult { value = a[0, 0], len = 1, x = 0, s = 0, y = 0 };
           

            int ii;

            for (ii = 0; ii < m - 1; ii++)
            {
                for (j = 0; j < nn; j++)
                    for (k = j; k < nn; k++)
                        f[j, k] = l[j, k] = 0;

                for (i = 0 + ii; i < m + ii; i++)
                    for (j = 0; j < nn; j++)
                        for (k = j, nx = 0; k < nn; k++, nx++)
                        {

                            if (nx >= n) break;


                            y = g[i, k] - g[i, j] + a[i, j];

                            if (f[j, k] > 0)
                            {
                                f[j, k] += y;
                                l[j, k]++;
                            }
                            else
                            {
                                f[j, k] = y;
                                l[j, k] = 1;
                            }


                            int z = (k - j) * l[j, k];

                            ///////// none vh
                            if (i < m && k < n)
                            {
                                if (nonevh.value < f[j, k] || nonevh.value == f[j, k] && (nonevh.y - nonevh.x) * (nonevh.len) > z)
                                {
                                    nonevh.value = f[j, k];
                                    nonevh.x = j;
                                    nonevh.y = k;
                                    nonevh.s = i;
                                    nonevh.len = l[j, k];
                                }
                            }





                            ///////////////////v
                            if (k < n)
                            {
                                if (v.value < f[j, k] || v.value == f[j, k] && (v.y - v.x) * v.len > z)
                                {
                                    v.value = f[j, k];
                                    v.x = j;
                                    v.y = k;
                                    v.s = i;
                                    v.len = l[j, k];
                                }
                            }

                            ///////////////////////h
                            if (i < m)
                            {
                                if (h.value < f[j, k] || h.value == f[j, k] && (h.y - h.x) * h.len > z)
                                {
                                    h.value = f[j, k];
                                    h.x = j;
                                    h.y = k;
                                    h.s = i;
                                    h.len = l[j, k];
                                }
                            }


                            ////////////////////vh
                            if (vh.value < f[j, k] || vh.value == f[j, k] && (vh.y - vh.x) * vh.len > z)
                            {
                                vh.value = f[j, k];
                                vh.x = j;
                                vh.y = k;
                                vh.s = i;
                                vh.len = l[j, k];
                            }

                        }
            }




            for (i = 0; i < mm; i++)
                for (j = 0; j < nn; j++)
                    tabitem[x].VHg[i, j] = tabitem[x].Vg[i, j] = tabitem[x].Hg[i, j] = tabitem[x].g[i, j] = false;

            for (i = 0; i < nonevh.len; i++)
            for (j = nonevh.x; j <= nonevh.y; j++)
                    tabitem[x].g[nonevh.s-i, j] = true;

            for (i = 0; i < vh.len; i++)
                for (j = vh.x; j <= vh.y; j++)
                    tabitem[x].VHg[vh.s - i, j] = true;

            for (i = 0; i < h.len; i++)
                for (j = h.x; j <= h.y; j++)
                    tabitem[x].Hg[h.s - i, j] = true;

            for (i = 0; i < v.len; i++)
                for (j = v.x; j <= v.y; j++)
                    tabitem[x].Vg[v.s - i, j] = true;

            tabitem[x].Hmaxsum = h.value;
            tabitem[x].Vmaxsum = v.value;
            tabitem[x].VHmaxsum = vh.value;
            tabitem[x].maxsum = nonevh.value;

            tabitem[x].filename = ss;




            bool[,] ttbool = new bool[m, n];
            int ttmaxsum = 0;
            
            
            
            
            AnyMaxsum tem = new AnyMaxsum(tabitem[x].m, tabitem[x].n, tabitem[x].ArrResource);


            int sumlen = 0;

            //GET NoneVH any G
            

            //set init
            tem.setvh(false, false);
            tem.sum = nonevh.value;
            tem.nsum = (nonevh.len) * (nonevh.y - nonevh.x + 1);


            //get value
            tem.GetResultData(ref ttmaxsum,ref ttbool);


            //set value
            for (i = 0; i < m; i++)
                for (j = 0; j < n;j++ )
                    tabitem[x].Ag[i,j] = ttbool[i,j];
            tabitem[x].Amaxsum = ttmaxsum;
            
            sumlen = tem.nsum;

            //GET V any G

            //set init
            tem.setvh(true, false);
            tem.sum = tabitem[x].Amaxsum;
            tem.nsum = sumlen;

            //get value
            tem.GetResultData(ref ttmaxsum, ref ttbool);

            //set value
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    tabitem[x].AVg[i, j] = ttbool[i, j];
            tabitem[x].AVmaxsum = ttmaxsum;


            //GET H any G

            tem.setvh(false, true);
            tem.sum = tabitem[x].Amaxsum;
            tem.nsum = sumlen;

            tem.GetResultData(ref ttmaxsum, ref ttbool);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    tabitem[x].AHg[i, j] = ttbool[i, j];
            tabitem[x].AHmaxsum = ttmaxsum;


            


            //GET VH any G
            tem.setvh(true, true);
            tem.GetResultData(ref ttmaxsum, ref ttbool);

            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    tabitem[x].AVHg[i, j] = ttbool[i, j];

            tabitem[x].AVHmaxsum = ttmaxsum;
            
            //GET cpu Load


            tabitem[x].Acpuload = tem.GetCpuLoad();

            



            setui();

        }


        //获取文件路径
        void getfile()
        {
            int x = UITabMain.SelectedIndex;
            if (x < 0)
            {
                MessageBox.Show("还没有选择任何Tab");
                return;
            }
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                getdata(filename);

            }
        }


        //UI
        void showaddnewfile()
        {
            AddNewFile.Visibility = Visibility.Visible;
            dataGrid.Visibility = Visibility.Collapsed;
        }

        void showdatagrid()
        {
            AddNewFile.Visibility = Visibility.Collapsed;
            dataGrid.Visibility = Visibility.Visible;
        }





        //响应UI的 几个方法

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;   
        }

        private void UITabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x=UITabMain.SelectedIndex;
            if(x<0) return ;
            if (x == UITabMain.Items.Count - 1)
            {
                UITabMain.Items.RemoveAt(x);
                
                ListBoxItem z = new ListBoxItem();
                TabItemDetail y = new TabItemDetail() { IsArrResourceGet = false };
                tabitem.Add(y);
                z.Content = (x + 1).ToString();
                z.HorizontalAlignment = HorizontalAlignment.Center;
                UITabMain.Items.Add(z);
                UITabMain.Items.Add(addnewitem);
                UITabMain.SelectedIndex=x;
                showaddnewfile();
                return;
            }
            if (tabitem[x].IsArrResourceGet)
            {
                setui();
                showdatagrid();
            }
            else
            {
                showaddnewfile();
            }
        }

        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            getfile();
        }

        private void UIResult_Click(object sender, RoutedEventArgs e)
        {
            int x = UITabMain.SelectedIndex;
            if (x < 0)
            {
                MessageBox.Show("没有任何数据");
                return;
            }
            if (!tabitem[x].IsArrResourceGet)
            {
                MessageBox.Show("还未选择数据源");
                return;
            }


            bool t1 = (bool)UIh.IsChecked;
            bool t2 = (bool)UIv.IsChecked;
            bool t3 = (bool)UIa.IsChecked;

            if (tabitem[x].IsHSelect == t1 && tabitem[x].IsVSelect == t2 && tabitem[x].IsASelect == t3)
            {
                MessageBox.Show("已经是这个结果了");
                return;
            }

            tabitem[x].IsHSelect = t1;
            tabitem[x].IsVSelect = t2;
            tabitem[x].IsASelect = t3;


            setui();

        }

        private void dataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                dataGrid.Columns[i].Header = (i + 1).ToString();

            }
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            (sender as DataGrid).Columns.Clear();
            for (int columnIndex = 0; columnIndex < this.CollectionSource[0].Count; columnIndex++)
            {
                DataGridTemplateColumn column = new DataGridTemplateColumn();
                XmlTextReader sr = new XmlTextReader(
                  new StringReader(
                    "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +
                      "<TextBlock Background=\"{Binding [" + columnIndex + "].Back}\" Text=\"{Binding [" + columnIndex + "].Value}\"  TextAlignment=\"Center\"  Foreground=\"Black\"/>" +
                    "</DataTemplate>"));

                column.CellTemplate = (DataTemplate)XamlReader.Load(sr);
                
                (sender as DataGrid).Columns.Add(column);
            }
            e.Column = null;
        }

        private void UIChangedatasource_Click(object sender, RoutedEventArgs e)
        {
            getfile();
        }



    }




    /// <summary>
    /// 需要用到的几个class
    /// </summary>

    //四个最大子矩阵：V,H,VH,nonevh,对应的数据
    public class maxsumresult
    {
        public int value { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int len { get; set; }
        public int s { get; set; }
    }


    //每一个Tab 对应的数据
    public class TabItemDetail
    {
        public string filename { get; set; }
        public int m { get; set; }
        public int n { get; set; }
        public bool IsArrResourceGet { get; set; }
        public int[,] ArrResource { get; set; }

        public bool IsVSelect { get; set; }
        public bool IsHSelect { get; set; }
        public bool IsASelect { get; set; }


        public int Hmaxsum { get; set; }
        public int VHmaxsum { get; set; }
        public int maxsum { get; set; }
        public int Vmaxsum { get; set; }
        public int Amaxsum { get; set; }
        public int AVmaxsum { get; set; }
        public int AHmaxsum { get; set; }
        public int AVHmaxsum { get; set; }
        public int Acpuload { get; set; }

        public bool[,] Vg { get; set; }
        public bool[,] Hg { get; set; }
        public bool[,] VHg { get; set; }
        public bool[,] Ag { get; set; }
        public bool[,] AHg { get; set; }
        public bool[,] AVg { get; set; }
        public bool[,] AVHg { get; set; }
        public bool[,] g { get; set; }
    }


    //UI datagrid 每一行的集合
    public class items : ObservableCollection<item> { }


    //UI datagrid 每一个cell  对应类
    public class item
    {
        public int Value { get; set; }
        public string Back { get; set; }
    }

}
