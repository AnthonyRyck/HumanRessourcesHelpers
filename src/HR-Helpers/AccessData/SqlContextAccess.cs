using AccessData.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccessData
{
	public class SqlContextAccess
	{
		public string ConnectionString { get; set; }

		public SqlContextAccess(string connectionString)
		{
			ConnectionString = connectionString;
		}

		#region Tableau

        /// <summary>
        /// Créé un nouveau tableau.
        /// </summary>
        /// <param name="nouveauTableau"></param>
        /// <returns></returns>
		public async Task AddTableau(Tableau nouveauTableau)
		{
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    // Création du Tableau
                    string command = "INSERT INTO tableau (IdTableau, IdUser, NomTableau, DescriptionTable, DateFinInscription)"
                                + " VALUES (@idTableau, @idUser, @nom, @description, @fin);";

                    using (var cmd = new MySqlCommand(command, conn))
                    {
                        cmd.Parameters.AddWithValue("@idTableau", nouveauTableau.IdTableau);
                        cmd.Parameters.AddWithValue("@idUser", nouveauTableau.IdUser);
                        cmd.Parameters.AddWithValue("@nom", nouveauTableau.NomDuTableau);
                        cmd.Parameters.AddWithValue("@description", nouveauTableau.Description);
                        cmd.Parameters.AddWithValue("@fin", nouveauTableau.DateFinInscription);

                        conn.Open();
                        int result = await cmd.ExecuteNonQueryAsync();
                        conn.Close();
                    }

                    // Création des colonnes
                    int maxLine = nouveauTableau.Colonnes.Count;
                    string commandInsertColonne = "INSERT INTO Colonne (IdColonne, TableId, NomColonne, DescriptionColonne, TypeData) VALUES ";
                    for (int i = 0; i < maxLine; i++)
                    {
                        commandInsertColonne += $"({nouveauTableau.Colonnes[i].IdColonne}" +
                                                $", '{nouveauTableau.Colonnes[i].TableId}'" +
                                                $", '{nouveauTableau.Colonnes[i].NomColonne}'" +
                                                $", '{nouveauTableau.Colonnes[i].Description}'" +
                                                $", '{nouveauTableau.Colonnes[i].TypeData}')";

                        if (i < (maxLine - 1))
                            commandInsertColonne += ", ";
                    }
                    commandInsertColonne += ";";

                    using (var cmd = new MySqlCommand(commandInsertColonne, conn))
                    {
                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public async Task CreateTables(string pathSql)
		{
			try
			{
                string cmd = await File.ReadAllTextAsync(pathSql);
                await ExecuteCoreAsync(cmd);
            }
            catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Supprime le tableau et les valeurs associés.
		/// </summary>
		/// <param name="idTableau"></param>
		/// <returns></returns>
		public async Task DeleteTableau(Guid idTableau)
		{
            // Suppression du tableau
            string commandDeleteTableau = $"DELETE FROM tableau "
                                        + $"WHERE IdTableau='{idTableau.ToString()}';";
            await ExecuteCoreAsync(commandDeleteTableau);
        }

        /// <summary>
        /// Charge le tableau avec l'ID donné en paramètre.
        /// </summary>
        /// <param name="idTableau"></param>
        /// <returns></returns>
        public async Task<Tableau> GetTableau(string idTableau)
		{

            var commandSelectTableau = @"SELECT IdTableau, IdUser, NomTableau, DescriptionTable, DateFinInscription "
                               + "FROM tableau "
                               + $"WHERE IdTableau='{idTableau}';";

            // Chargement de l'objet Tableau
            Func<MySqlCommand, Task<Tableau>> funcCmd = async (cmd) =>
            {
                Tableau tableauResult = null;
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        tableauResult = new Tableau()
                        {
                            IdTableau = new Guid(reader.GetString(0)),
                            IdUser = reader.GetString(1),
                            NomDuTableau = reader.GetString(2),
                            Description = reader.GetString(3),
                            DateFinInscription = reader.GetDateTime(4)
                        };
                    }
                }

                return tableauResult;
            };

            // Chargement des colonnes pour ce tableau
            var commandColonnes = @"SELECT col.IdColonne, col.NomColonne, col.DescriptionColonne, col.TypeData "
                                    + "FROM colonne col "
                                    + $"WHERE col.TableId='{idTableau}';";
            Func<MySqlCommand, Task<List<ColonneModel>>> funcCmdColonne = async (cmd) =>
            {
                List<ColonneModel> colonnes = new List<ColonneModel>();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var colonne = new ColonneModel()
                        {
                            IdColonne = reader.GetInt32(0),
                            NomColonne = reader.GetString(1),
                            Description = reader.GetString(2),
                            TypeData = reader.GetString(3),
                            TableId = new Guid(idTableau)                            
                        };

                        colonnes.Add(colonne);
                    }
                }

                return colonnes;
            };


            try
            {
                var tableau = await GetCoreAsync<Tableau>(commandSelectTableau, funcCmd);

                if (tableau == null)
                    return null;

                var colonnes = await GetCoreAsync<List<ColonneModel>>(commandColonnes, funcCmdColonne);

                tableau.Colonnes = colonnes;

                return tableau;
            }
            catch (Exception ex)
            {
                var exs = ex.Message;
                throw;
            }
        }

		/// <summary>
		/// Charge tous les tableaux d'un utilisateur.
		/// </summary>
		/// <param name="idUser"></param>
		/// <returns></returns>
		public async Task<IEnumerable<Tableau>> GetTableauByUser(string idUser)
		{
            var commandText = @"SELECT IdTableau, IdUser, NomTableau, DescriptionTable, DateFinInscription "
                                + "FROM tableau "
                                + $"WHERE IdUser='{idUser}';";

            Func<MySqlCommand, Task<List<Tableau>>> funcCmd = async (cmd) =>
            {
                List<Tableau> tableaux = new List<Tableau>();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var tableau = new Tableau()
                        {
                            IdTableau = new Guid(reader.GetString(0)),
                            IdUser = reader.GetString(1),
                            NomDuTableau = reader.GetString(2),
                            Description = reader.GetString(3),
                            DateFinInscription = reader.GetDateTime(4)
                        };

                        tableaux.Add(tableau);
                    }
                }

                return tableaux;
            };

            List<Tableau> tableaux = new List<Tableau>();

            try
            {
                tableaux = await GetCoreAsync(commandText, funcCmd);
            }
            catch (Exception ex)
            {
                var exs = ex.Message;
            }

            return tableaux;
        }

        #endregion

        #region Valeurs

        /// <summary>
        /// Récupère les valeurs pour le tableau et l'utilisateur.
        /// </summary>
        /// <param name="idTableau"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<List<ValueColonne>>> GetValeurs(string idTableau, string userId)
        {
            var commandText = @"SELECT NumeroLigne, ColonneId, TableId, UserId, Valeur "
                                + "FROM valeur "
                                + $"WHERE UserId='{userId}' " 
                                + $"AND TableId='{idTableau}' " 
                                + $"ORDER BY NumeroLigne, ColonneId;";

            Func<MySqlCommand, Task<List<List<ValueColonne>>>> funcCmd = async (cmd) =>
            {
                List<List<ValueColonne>> toutesLesLignes = new List<List<ValueColonne>>();

                List<ValueColonne> uneLigne = new List<ValueColonne>();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int numeroLigne = 0;

                    while (reader.Read())
                    {
                        var valeur = new ValueColonne()
                        {
                            NumeroLigne = reader.GetInt32(0),
                            IdColonne = reader.GetInt32(1),
                            IdTableau = new Guid(reader.GetString(2)),
                            IdUser = reader.GetString(3),
                            Value = reader.GetString(4)
                        };

                        if(numeroLigne == 0 || numeroLigne == valeur.NumeroLigne)
						{
                            uneLigne.Add(valeur);
                        }
                        else
						{
                            // Ajout la collection de la ligne X
                            toutesLesLignes.Add(uneLigne);

                            // Renouvelle pour la ligne Y
                            uneLigne = new List<ValueColonne>();
                            uneLigne.Add(valeur);
                        }

                        numeroLigne = valeur.NumeroLigne;
                    }

                    // Comme pas d'autre numéro de ligne, j'ajoute les enregistrements.
                    if(uneLigne.Count > 0)
                        toutesLesLignes.Add(uneLigne);
                }

                return toutesLesLignes;
            };

            List<List<ValueColonne>> result = new List<List<ValueColonne>>();

            try
            {
                result = await GetCoreAsync(commandText, funcCmd);
            }
            catch (Exception ex)
            {
                var exs = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Récupère les valeurs pour le tableau et l'utilisateur.
        /// </summary>
        /// <param name="idTableau"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<List<ValueColonne>>> GetValeurs(string idTableau)
        {
            var commandText = @"SELECT NumeroLigne, ColonneId, TableId, UserId, Valeur "
                                + "FROM valeur "
                                + $"WHERE TableId='{idTableau}' "
                                + $"ORDER BY UserId, NumeroLigne, ColonneId;";

            Func<MySqlCommand, Task<List<List<ValueColonne>>>> funcCmd = async (cmd) =>
            {
                List<List<ValueColonne>> toutesLesLignes = new List<List<ValueColonne>>();

                List<ValueColonne> uneLigne = new List<ValueColonne>();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int numeroLigne = 0;

                    while (reader.Read())
                    {
                        var valeur = new ValueColonne()
                        {
                            NumeroLigne = reader.GetInt32(0),
                            IdColonne = reader.GetInt32(1),
                            IdTableau = new Guid(reader.GetString(2)),
                            IdUser = reader.GetString(3),
                            Value = reader.GetString(4)
                        };

                        if (numeroLigne == 0 || numeroLigne == valeur.NumeroLigne)
                        {
                            uneLigne.Add(valeur);
                        }
                        else
                        {
                            // Ajout la collection de la ligne X
                            toutesLesLignes.Add(uneLigne);

                            // Renouvelle pour la ligne Y
                            uneLigne = new List<ValueColonne>();
                            uneLigne.Add(valeur);
                        }

                        numeroLigne = valeur.NumeroLigne;
                    }

                    // Comme pas d'autre numéro de ligne, j'ajoute les enregistrements.
                    if (uneLigne.Count > 0)
                        toutesLesLignes.Add(uneLigne);
                }

                return toutesLesLignes;
            };

            List<List<ValueColonne>> result = new List<List<ValueColonne>>();

            try
            {
                result = await GetCoreAsync(commandText, funcCmd);
            }
            catch (Exception ex)
            {
                var exs = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Ajoute une nouvelle ligne de valeur pour un tableau
        /// </summary>
        /// <param name="valeurs"></param>
        /// <returns></returns>
        public async Task AddValeurs(List<ValueColonne> valeurs)
        {
			try
			{
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    //// Création du Tableau
                    //string command = "INSERT INTO valeur (ColonneId, TableId, UserId, Valeur)"
                    //            + " VALUES (@colonneId, @tableId, @userId, @valeur);";

                    //using (var cmd = new MySqlCommand(command, conn))
                    //{
                    //    cmd.Parameters.AddWithValue("@colonneId", nouveauTableau.IdTableau);
                    //    cmd.Parameters.AddWithValue("@tableId", nouveauTableau.IdUser);
                    //    cmd.Parameters.AddWithValue("@userId", nouveauTableau.NomDuTableau);
                    //    cmd.Parameters.AddWithValue("@valeur", nouveauTableau.Description);

                    //    conn.Open();
                    //    int result = await cmd.ExecuteNonQueryAsync();
                    //    conn.Close();
                    //}

                    // Insertion des valeurs
                    int maxLine = valeurs.Count;
                    string commandInsertValeur = "INSERT INTO valeur (NumeroLigne, ColonneId, TableId, UserId, Valeur) VALUES ";
                    for (int i = 0; i < maxLine; i++)
                    {
                        commandInsertValeur += $"({valeurs[i].NumeroLigne}" +
                                                $", {valeurs[i].IdColonne}" +
                                                $", '{valeurs[i].IdTableau}'" +
                                                $", '{valeurs[i].IdUser}'" +
                                                $", '{valeurs[i].Value}')";

                        if (i < (maxLine - 1))
                            commandInsertValeur += ", ";
                    }
                    commandInsertValeur += ";";

                    using (var cmd = new MySqlCommand(commandInsertValeur, conn))
                    {
                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        conn.Close();
                    }
                }
            }
			catch (Exception ex)
			{

				throw;
			}
        }

        /// <summary>
        /// Supprime une ligne de valeur.
        /// </summary>
        /// <param name="idTableau"></param>
        /// <param name="idUser"></param>
        /// <param name="numeroLigne"></param>
        /// <returns></returns>
        public async Task DeleteRow(string idTableau, string idUser, int numeroLigne)
        {
            // Suppression d'une ligne
            string commandDeleteValeur = $"DELETE FROM valeur "
                                        + $"WHERE TableId='{idTableau}' "
                                        + $"AND UserId='{idUser}' "
                                        + $"AND NumeroLigne={numeroLigne} ;";
            await ExecuteCoreAsync(commandDeleteValeur);
        }

        /// <summary>
        /// Met à une une ligne de valeur.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateValeurs(List<ValueColonne> value)
        {
            foreach (var valeur in value)
            {
                string cmdUpdate = $"UPDATE valeur "
                    + $"SET valeur='{valeur.Value}' "
                    + $"WHERE NumeroLigne={valeur.NumeroLigne} "
                    + $"AND TableId='{valeur.IdTableau}' "
                    + $"AND UserId='{valeur.IdUser}' "
                    + $"AND ColonneId={valeur.IdColonne};";

                await ExecuteCoreAsync(cmdUpdate);
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<T>> GetCoreAsync<T>(string commandSql, Func<MySqlCommand, Task<List<T>>> func)
            where T : new()
        {
            List<T> result = new List<T>();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(commandSql, conn);
                    conn.Open();
                    result = await func.Invoke(cmd);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandSql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private async Task<T> GetCoreAsync<T>(string commandSql, Func<MySqlCommand, Task<T>> func)
            where T : new()
        {
            T result = new T();

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(commandSql, conn);
                    conn.Open();
                    result = await func.Invoke(cmd);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Execute une commande qui n'attend pas de retour.
        /// </summary>
        /// <param name="commandSql"></param>
        /// <returns></returns>
        private async Task ExecuteCoreAsync(string commandSql)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(commandSql, conn);

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Permet la récupération d'un BLOB uniquement !
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private async Task<byte[]> GetBytesCore(string commandText)
        {
            byte[] file = null;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            file = (byte[])reader[0];

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return file;
        }

        /// <summary>
        /// Permet la récupération d'un ID type int uniquement !
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private async Task<int> GetIntCore(string commandText)
        {
            int id = 0;

            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(commandText, conn);

                    UInt64 idTemp = 0;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            idTemp = (UInt64)reader[0];
                        }
                    }

                    id = Convert.ToInt32(idTemp);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return id;
        }

        /// <summary>
        /// Permet de gérer les retours de valeur null de la BDD
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        #endregion
    }
}
