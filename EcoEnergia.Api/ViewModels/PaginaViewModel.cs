namespace EcoEnergia.Api.ViewModels
{
    // Resposta padrão das listagens paginadas (todos os controllers vão usar)
    public class PaginaViewModel<T>
    {
        public IEnumerable<T> Itens { get; set; } = new List<T>();
        public int Pagina { get; set; }
        public int Tamanho { get; set; }
        public int TotalItens { get; set; }

        // Calcula quantas páginas existem no total
        public int TotalPaginas => (int)Math.Ceiling(TotalItens / (double)Tamanho);
    }
}