namespace LtdService.CommonServices
{
    public interface IModelDataAnnotationCheck
    {
        void ValidateModelDataAnnotations<TDomainModel>(TDomainModel domainModel);
    }
}