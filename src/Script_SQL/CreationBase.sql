CREATE DATABASE IF NOT EXISTS hrhelpersdb;
USE hrhelpersdb;

CREATE TABLE Tableau
(IdTableau VARCHAR(255) NOT NULL, 
IdUser VARCHAR(255) NOT NULL, 
NomTableau VARCHAR(250) NOT NULL,
DescriptionTable VARCHAR(250),
DateFinInscription DATE NOT NULL,
PRIMARY KEY(IdTableau, IdUser));

CREATE TABLE Colonne
(IdColonne INT NOT NULL, 
TableId VARCHAR(255) NOT NULL, 
NomColonne VARCHAR(250) NOT NULL,
DescriptionColonne VARCHAR(250),
TypeData VARCHAR(30),
PRIMARY KEY(IdColonne,TableId),
FOREIGN KEY(TableId) REFERENCES Tableau(IdTableau));

CREATE TABLE Valeur
(ColonneId INT NOT NULL, 
TableId VARCHAR(255) NOT NULL,
UserId VARCHAR(255) NOT NULL,  
Valeur VARCHAR(255) NOT NULL,
DescriptionColonne VARCHAR(250),
TypeData VARCHAR(30),
PRIMARY KEY(ColonneId,TableId),
FOREIGN KEY(TableId) REFERENCES Tableau(IdTableau),
FOREIGN KEY(ColonneId) REFERENCES Colonne(IdColonne));