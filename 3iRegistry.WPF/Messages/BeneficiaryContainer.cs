using _3iRegistry.Core;
using System.Collections.ObjectModel;

namespace _3iRegistry.WPF.Messages
{
    /// <summary>
    /// This Class acts as a global singleton container for the different views
    /// in order to access the same beneficiary details and state (edit|add mode)
    /// </summary>
    public class BeneficiaryContainer
    {
        private static readonly object locker = new object();
        private static BeneficiaryContainer _instance;

        public static BeneficiaryContainer Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                            _instance = new BeneficiaryContainer();
                    }
                }
                return _instance;
            }
        }

        // Enforcy access to object via getter providing only a single instance
        private BeneficiaryContainer() { }

        public Beneficiary SelectedBeneficiary { get; set; }
        public ObservableCollection<Partner> SelectedPartners { get; set; }
        public ObservableCollection<Learner> SelectedLearners { get; set; }
        public ObservableCollection<Furniture> SelectedFurniture { get; set; }
        public bool IsEditMode { get; set; } = false;
        public bool IsEditPartner { get; set; }
        public bool IsEditLearner { get; set; } = false;
        public bool IsEditFurniture { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
    }
}
