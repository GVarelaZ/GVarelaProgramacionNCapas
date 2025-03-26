using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUsuario" in both code and config file together.
    [ServiceContract] //Asigna que funciones u operaciones visibles 
    public interface IUsuario
    {
        [OperationContract] //Publice el metodo al usuario que consume el web service
         SL_WCF.Result AddWebService(ML.Usuario usuario); //firma de contrato

        [OperationContract]
        SL_WCF.Result UpdateWebService(ML.Usuario usuario);

        [OperationContract]
        SL_WCF.Result DeleteWebService(int idUsuario);

        [OperationContract]
        [ServiceKnownType(typeof(ML.Usuario))]
        SL_WCF.Result GetAllWebService(ML.Usuario usuario);

        [OperationContract]
        [ServiceKnownType(typeof(ML.Usuario))]
        SL_WCF.Result GetByIdWebService(int idUsuario);
    }
}
