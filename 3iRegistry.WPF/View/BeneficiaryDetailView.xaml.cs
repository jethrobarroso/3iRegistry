using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3iRegistry.WPF.View
{
    /// <summary>
    /// Interaction logic for BeneficiaryDetailView.xaml
    /// </summary>
    public partial class BeneficiaryDetailView : UserControl
    {
        public BeneficiaryDetailView()
        {
            InitializeComponent();
        }

        private void AdjustUnemployed(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            var learnersCount = listboxLeaners.Items.Count;
            var householdUnemployedDiff = numTotalHousehold.Value - learnersCount;

            if (numUnemployed.Value > householdUnemployedDiff)
            {
                numUnemployed.Value = householdUnemployedDiff;
                numUnemployed.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            }
        }
    }
}
