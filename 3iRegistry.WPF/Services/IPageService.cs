using _3iRegistry.WPF.View;
using System.Windows.Controls;

namespace _3iRegistry.WPF.Services
{
    public interface IPageService
    {
        UserControl GetBeneficiaryDetailsPage();
        UserControl GetDashboardPage();
        PartnerDetailView GetPartnerDetailDialog();
        FurnitureDetailView GetFurnitureDetailDialog();
        LearnerDetailView GetLearnerDetailDialog();
    }
}