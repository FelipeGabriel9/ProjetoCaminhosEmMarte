using System;
using System.Collections.Generic;

public interface IHashing<T> 
                 where T : IRegistro<T>, IEquatable<T>, new()
{
    bool Incluiu(T novoDado);
    bool Excluiu(T dadoAExcluir);
    bool Existe(T dadoAProcurar, out int onde);
    List<string> LocaisDosDados();
    List<T> Conteudo();
}
