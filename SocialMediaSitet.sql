USE [master]
GO
/****** Object:  Database [SocialMediaSite]    Script Date: 12/12/2021 22:23:13 ******/
CREATE DATABASE [SocialMediaSite]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SocialMediaSite', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SocialMediaSite.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SocialMediaSite_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SocialMediaSite_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SocialMediaSite] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SocialMediaSite].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SocialMediaSite] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SocialMediaSite] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SocialMediaSite] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SocialMediaSite] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SocialMediaSite] SET ARITHABORT OFF 
GO
ALTER DATABASE [SocialMediaSite] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SocialMediaSite] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SocialMediaSite] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SocialMediaSite] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SocialMediaSite] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SocialMediaSite] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SocialMediaSite] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SocialMediaSite] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SocialMediaSite] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SocialMediaSite] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SocialMediaSite] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SocialMediaSite] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SocialMediaSite] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SocialMediaSite] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SocialMediaSite] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SocialMediaSite] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SocialMediaSite] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SocialMediaSite] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SocialMediaSite] SET  MULTI_USER 
GO
ALTER DATABASE [SocialMediaSite] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SocialMediaSite] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SocialMediaSite] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SocialMediaSite] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SocialMediaSite] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SocialMediaSite] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SocialMediaSite] SET QUERY_STORE = OFF
GO
USE [SocialMediaSite]
GO
/****** Object:  Table [dbo].[tbl_Benutzer]    Script Date: 12/12/2021 22:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Benutzer](
	[id_Benutzer] [int] IDENTITY(1,1) NOT NULL,
	[Benutzername] [nchar](20) NOT NULL,
	[Passwort] [nchar](20) NOT NULL,
	[isAdmin] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Benutzer] PRIMARY KEY CLUSTERED 
(
	[id_Benutzer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BenutzerBenutzer]    Script Date: 12/12/2021 22:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BenutzerBenutzer](
	[id_BenutzerBenutzer] [int] IDENTITY(1,1) NOT NULL,
	[fk_id_BenutzerFolgen] [int] NOT NULL,
	[fk_id_BenutzerGefolgt] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_BenutzerBenutzer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BenutzerKategorie]    Script Date: 12/12/2021 22:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BenutzerKategorie](
	[id_BenutzerKategorie] [int] IDENTITY(1,1) NOT NULL,
	[fk_id_Benutzer] [int] NOT NULL,
	[fk_id_Kategorie] [int] NOT NULL,
 CONSTRAINT [PK_BenutzerKategorie] PRIMARY KEY CLUSTERED 
(
	[id_BenutzerKategorie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Kategorie]    Script Date: 12/12/2021 22:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Kategorie](
	[id_Kategorie] [int] IDENTITY(1,1) NOT NULL,
	[KategorieName] [nchar](15) NOT NULL,
 CONSTRAINT [PK_tbl_Kategorie] PRIMARY KEY CLUSTERED 
(
	[id_Kategorie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_logs]    Script Date: 12/12/2021 22:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_logs](
	[id_Log] [int] NOT NULL,
	[LogInfo] [ntext] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Posts]    Script Date: 12/12/2021 22:23:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Posts](
	[id_Post] [int] IDENTITY(1,1) NOT NULL,
	[Titel] [nchar](30) NOT NULL,
	[Inhalt] [nchar](140) NOT NULL,
	[PostDate] [datetime] NOT NULL,
	[fk_id_Kategorie] [int] NOT NULL,
	[fk_id_Benutzer] [int] NOT NULL,
 CONSTRAINT [PK_tbl_Posts] PRIMARY KEY CLUSTERED 
(
	[id_Post] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_BenutzerBenutzer]  WITH CHECK ADD FOREIGN KEY([fk_id_BenutzerFolgen])
REFERENCES [dbo].[tbl_Benutzer] ([id_Benutzer])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_BenutzerBenutzer]  WITH CHECK ADD FOREIGN KEY([fk_id_BenutzerGefolgt])
REFERENCES [dbo].[tbl_Benutzer] ([id_Benutzer])
GO
ALTER TABLE [dbo].[tbl_BenutzerKategorie]  WITH CHECK ADD  CONSTRAINT [fk_Benutzer_BenutzerKategorie] FOREIGN KEY([fk_id_Benutzer])
REFERENCES [dbo].[tbl_Benutzer] ([id_Benutzer])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_BenutzerKategorie] CHECK CONSTRAINT [fk_Benutzer_BenutzerKategorie]
GO
ALTER TABLE [dbo].[tbl_BenutzerKategorie]  WITH CHECK ADD  CONSTRAINT [fk_Kategorie_BenutzerKategorie] FOREIGN KEY([fk_id_Kategorie])
REFERENCES [dbo].[tbl_Kategorie] ([id_Kategorie])
GO
ALTER TABLE [dbo].[tbl_BenutzerKategorie] CHECK CONSTRAINT [fk_Kategorie_BenutzerKategorie]
GO
ALTER TABLE [dbo].[tbl_Posts]  WITH CHECK ADD  CONSTRAINT [FK_Benutzer_Post] FOREIGN KEY([fk_id_Benutzer])
REFERENCES [dbo].[tbl_Benutzer] ([id_Benutzer])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Posts] CHECK CONSTRAINT [FK_Benutzer_Post]
GO
ALTER TABLE [dbo].[tbl_Posts]  WITH CHECK ADD  CONSTRAINT [FK_Kategorie_Post] FOREIGN KEY([fk_id_Kategorie])
REFERENCES [dbo].[tbl_Kategorie] ([id_Kategorie])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_Posts] CHECK CONSTRAINT [FK_Kategorie_Post]
GO
USE [master]
GO
ALTER DATABASE [SocialMediaSite] SET  READ_WRITE 
GO
