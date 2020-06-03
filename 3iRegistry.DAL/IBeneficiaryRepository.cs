using _3iRegistry.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3iRegistry.DAL
{
    /// <summary>
    /// Interface for implementing crud operations to a data source
    /// </summary>
    public interface IBeneficiaryRepository
    {
        Task<List<Beneficiary>> GetBeneficiaries();
        Beneficiary GetBeneficiaryById(string id);
        Beneficiary DeleteBeneficiary(Beneficiary beneficiary);
        Task<Beneficiary> UpdateBeneficiary(Beneficiary beneficiary);
        Beneficiary AddBeneficiary(Beneficiary beneficiary);
        string AddSchool(string school);
        string AddSettlement(string settlement);
        List<string> GetSchools();
        List<string> GetSettlements();
        IEnumerable<User> GetUsers();
    }
}
