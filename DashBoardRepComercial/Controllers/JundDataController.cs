using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DashBoardRepComercial.Models;

namespace DashBoardRepComercial.Controllers
{
    public class JundDataController : Controller
    {
        static JUNDSOFTEntities jund = new JUNDSOFTEntities();

        public static T_SY_40 GetVendedor(String usuarioLogado)
        {
            var user = jund.T_SY_42.Where(u => u.SY42_USUARIO == (string)usuarioLogado).FirstOrDefault();
            var grupo = jund.T_SY_43.Where(g => g.SY42_USUARIO.Equals(user.SY42_USUARIO)).FirstOrDefault();
            return jund.T_SY_40.Where(v => v.SY40_GRUPO.Equals(grupo.SY40_GRUPO) && v.SY01_EMPRESA == "11").FirstOrDefault();
        }

        public static IEnumerable<produtosABC> GetProdutos(string vend)
        {
            return jund.Database.SqlQuery<produtosABC>(String.Format("SELECT      ROW_NUMBER() OVER(order by SUM(B.FT02_VLR_TOTAL) DESC) AS 'SEQ',      p.cd25_vendedor,      D.CD04_MATERIAL AS 'Industrial',      D.CD04_APLICACAO AS 'Merc',      D.CD04_DESCRICAO AS 'Produto',      cast(SUM(CASE WHEN A.FT01_TIPO = 110 THEN 0 ELSE B.FT02_QTDE END) as numeric (36,0) )AS 'Qtde_vendida',      SUM(B.FT02_VLR_TOTAL) as 'Valor_Vendido',		cast((SUM(B.FT02_VLR_TOTAL) 		  /(Select SUM(B.FT02_VLR_TOTAL) From T_FT_02 B	JOIN  T_FT_01 A ON (B.SY01_EMPRESA   = A.SY01_EMPRESA) 	AND (B.FT01_NOTA      = A.FT01_NOTA)	AND (B.CD14_SIGLA     = A.CD14_SIGLA)	JOIN  T_CD_08 C ON      (C.CD08_CGC_CLI   = A.CD08_CGC_CLI)JOIN  T_CD_04 D ON      (D.CD04_MATERIAL = B.CD04_MATERIAL)JOIN  T_CD_05 E ON      (E.SY01_EMPRESA   = B.SY01_EMPRESA AND E.CD04_MATERIAL = B.CD04_MATERIAL)JOIN  T_PV_01 P ON		(P.PV01_PEDIDO = B.PV01_PEDIDO)		where (				(A.SY01_EMPRESA 		= '11')			AND				(A.FT01_TIPO 		< 111)			AND				(A.FT01_CANCELADA 	= 'N')			AND								(A.FT01_DATA_EMISSAO BETWEEN CONVERT(DATETIME,'{0}',103) AND CONVERT(DATETIME,'{1}',103))	AND							(p.cd25_vendedor = '{2}')						)) * 100) as numeric (36,3)) as 'ABC'FROM      T_FT_01 A JOIN  T_FT_02 B ON (B.SY01_EMPRESA   = A.SY01_EMPRESA) 			AND (B.FT01_NOTA      = A.FT01_NOTA)			AND (B.CD14_SIGLA     = A.CD14_SIGLA)JOIN  T_CD_08 C ON      (C.CD08_CGC_CLI   = A.CD08_CGC_CLI)JOIN  T_CD_04 D ON      (D.CD04_MATERIAL = B.CD04_MATERIAL)JOIN  T_CD_05 E ON      (E.SY01_EMPRESA   = B.SY01_EMPRESA AND E.CD04_MATERIAL = B.CD04_MATERIAL)JOIN  T_PV_01 P ON		(P.PV01_PEDIDO = B.PV01_PEDIDO)WHERE (A.SY01_EMPRESA         = '11') AND      (A.FT01_TIPO            < 111) AND      (A.FT01_CANCELADA       = 'N') AND	(A.FT01_DATA_EMISSAO BETWEEN CONVERT(DATETIME,'{0}',103) AND CONVERT(DATETIME,'{1}',103))	AND	(p.cd25_vendedor = '{2}')group by p.cd25_vendedor,D.CD04_MATERIAL,D.CD04_APLICACAO,D.CD04_DESCRICAO order by 'Valor_Vendido' desc;"
                        , new DateTime(DateTime.Now.AddYears(-11).Year, DateTime.Now.Month, 1)
                        , DateTime.Now, vend)).ToList<produtosABC>().Take(10);
        }

        public static IEnumerable<Ults_Fats_Vend_Result> GetFaturamentos(string vend)
        {
            var t = jund.Database.SqlQuery<Ults_Fats_Vend_Result>(String.Format("SELECT       TOP(10)       ROW_NUMBER() OVER(ORDER BY C.CD08_ULTIMO_FAT DESC) AS 'SEQ',       C.CD08_NOME_CLI AS 'Cliente',       C.CD30_uf AS 'UF',       CD08_ULTIMO_FAT  AS 'Ult_Fat',       MONTH(C.CD08_CADASTRO) AS 'Mes_Cadastro',       YEAR(C.CD08_CADASTRO) AS 'Ano_Cadastro',       SUM(CASE WHEN  B.SY01_EMPRESA = '11' and A.FT01_TIPO < 111 then B.FT02_VLR_TOTAL else 0 end) AS 'Valor'       FROM       T_FT_01 A JOIN   T_FT_02 B ON (B.SY01_EMPRESA     = A.SY01_EMPRESA)   AND                    (B.FT01_NOTA = A.FT01_NOTA)             AND                    (B.CD14_SIGLA = A.CD14_SIGLA)JOIN   T_CD_08 C ON (C.CD08_CGC_CLI     = A.CD08_CGC_CLI)JOIN   T_CD_04 D ON (D.CD04_MATERIAL = B.CD04_MATERIAL)JOIN   T_CD_05 E ON (E.SY01_EMPRESA     = B.SY01_EMPRESA AND E.CD04_MATERIAL = B.CD04_MATERIAL)WHERE  (A.SY01_EMPRESA            = '11')                    AND       (A.FT01_TIPO        < 111)              AND       (A.FT01_CANCELADA   = 'N')              AND       (A.FT01_DATA_EMISSAO BETWEEN CONVERT(DATETIME,'{0}',103) AND CONVERT(DATETIME,'{1}',103)) AND       (A.CD25_VENDEDOR = '{2}')group by C.CD08_NOME_CLI, C.CD30_uf, C.CD08_ULTIMO_FAT, C.CD08_CADASTRO order by 'Ult_Fat' DESC"
                        , new DateTime(DateTime.Now.AddYears(-11).Year, DateTime.Now.Month, 1)
                        , DateTime.Now, vend)).ToList<Ults_Fats_Vend_Result>();
            Console.WriteLine("");
            return t;
        }

        public static IQueryable<T_FT_01> GetFaturamentosGerRegional(string ger)
        {
            var vendedores = jund.T_CD_25.Where(v => v.CD34_GERENTE == ger && v.SY01_EMPRESA == "11").ToList();
            List<T_FT_01> faturamentos = new List<T_FT_01>();
            foreach (var vendedor in vendedores)
            {
                var fatur = jund.T_FT_01.Where(fat => fat.CD25_VENDEDOR == vendedor.CD25_VENDEDOR && fat.FT01_TIPO < 111)
                    .OrderByDescending(fat => fat.FT01_DATA_EMISSAO).Take(1).ToList();
                for (int i = 0; i < fatur.Count; i++)
                {
                    faturamentos.Add(fatur.ElementAt(i));
                }
            }
            faturamentos.AsQueryable().OrderByDescending(fat => fat.FT01_DATA_EMISSAO).Take(10);

            return faturamentos.AsQueryable().OrderByDescending(fat => fat.FT01_DATA_EMISSAO).Take(10);
        }
    }
}