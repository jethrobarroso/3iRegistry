using _3iRegistry.WPF.View;
using System.Windows.Controls;

namespace _3iRegistry.WPF.Services
{
    public interface IPageService
    {
        UserControl ShowBeneficiaryDetailsView();
        UserControl ShowDashboardView();
        PartnerDetailView ShowPartnerDetailDialog();
        FurnitureDetailView ShowFurnitureDetailDialog();
        LearnerDetailView ShowLearnerDetailDialog();
        void ShowMainView();
        void ShowLoginView();
    }
}