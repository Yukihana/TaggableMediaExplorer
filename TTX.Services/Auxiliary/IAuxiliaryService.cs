using TTX.Data.Entities;

namespace TTX.Services.Auxiliary;

public interface IAuxiliaryService
{
    bool AddDuplicateFile(string path);

    bool RemoveDuplicateFile(string path);

    bool AddDuplicateRecords(string identity, AssetRecord rec);

    bool RemoveDuplicateRecords(AssetRecord rec);
}