USE [master]
GO
/****** Object:  Database [WarehouseDB]    Script Date: 1/11/2017 10:42:58 AM ******/
CREATE DATABASE [WarehouseDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WarehouseDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\WarehouseDB.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'WarehouseDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\WarehouseDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [WarehouseDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WarehouseDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WarehouseDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WarehouseDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WarehouseDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WarehouseDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WarehouseDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [WarehouseDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WarehouseDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WarehouseDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WarehouseDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WarehouseDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WarehouseDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WarehouseDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WarehouseDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WarehouseDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WarehouseDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WarehouseDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WarehouseDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WarehouseDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WarehouseDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WarehouseDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WarehouseDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WarehouseDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WarehouseDB] SET RECOVERY FULL 
GO
ALTER DATABASE [WarehouseDB] SET  MULTI_USER 
GO
ALTER DATABASE [WarehouseDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WarehouseDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WarehouseDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WarehouseDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [WarehouseDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [WarehouseDB]
GO
/****** Object:  Table [dbo].[Consignment]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Consignment](
	[id] [int] NOT NULL,
	[warehouseId] [int] NULL,
	[supplier] [varchar](max) NULL,
	[totalItems] [int] NULL,
	[arrivalDate] [datetime] NULL,
	[consignmentName] [varchar](max) NULL,
 CONSTRAINT [PK_Consignment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item](
	[id] [int] NOT NULL,
	[itemName] [varchar](max) NULL,
	[Manufacturer] [varchar](max) NULL,
	[Country] [varchar](max) NULL,
	[itemCode] [varchar](max) NULL,
	[Category] [varchar](max) NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Item_Consignment]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Consignment](
	[itemId] [int] NOT NULL,
	[consignmentId] [int] NOT NULL,
	[quantity] [int] NULL,
	[expiry] [date] NULL,
 CONSTRAINT [PK_item_consignment] PRIMARY KEY CLUSTERED 
(
	[itemId] ASC,
	[consignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Item_Warehouse]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Warehouse](
	[itemId] [int] NOT NULL,
	[warehouseId] [int] NOT NULL,
	[quantity] [int] NULL,
	[orders] [int] NULL,
 CONSTRAINT [PK_Item_Warehouse] PRIMARY KEY CLUSTERED 
(
	[itemId] ASC,
	[warehouseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[orderId] [int] NOT NULL,
	[transactionId] [int] NOT NULL,
	[customerId] [int] NULL,
	[orderDate] [date] NULL,
	[orderStatus] [varchar](max) NULL,
	[warehouseId] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC,
	[transactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Shelf]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Shelf](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[shelfName] [varchar](max) NULL,
	[warehouse_id] [int] NULL,
	[zone] [varchar](max) NULL,
	[shelfItems] [varchar](max) NULL,
	[slotsRemaining] [int] NULL,
 CONSTRAINT [PK_Shelf] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[itemId] [int] NOT NULL,
	[warehouseId] [int] NULL,
	[quantity] [int] NULL,
	[shelfId] [int] NULL,
	[slotNumber] [int] NULL,
	[transactionDate] [date] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccounts](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Contact] [nvarchar](11) NOT NULL,
	[Registered] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[ConfirmPassword] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserAccounts] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Warehouse]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Warehouse](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[warehouseHTML] [varbinary](max) NULL,
	[actualWarehouseWidth] [float] NULL,
	[actualWarehouseLength] [float] NULL,
	[scaledWarehouseWidth] [float] NULL,
	[scaledWarehouseLength] [float] NULL,
	[actualShelfHeight] [float] NULL,
	[actualShelfWidth] [float] NULL,
	[actualShelfLength] [int] NULL,
	[scaledShelfWidth] [float] NULL,
	[scaledShelfLength] [float] NULL,
	[shelves] [int] NULL,
	[shelfRows] [int] NULL,
	[shelfSlots] [int] NULL,
	[managerId] [int] NULL,
	[sections] [int] NULL,
 CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Item_Consignment]  WITH CHECK ADD  CONSTRAINT [FK_consignment] FOREIGN KEY([consignmentId])
REFERENCES [dbo].[Consignment] ([id])
GO
ALTER TABLE [dbo].[Item_Consignment] CHECK CONSTRAINT [FK_consignment]
GO
ALTER TABLE [dbo].[Item_Consignment]  WITH CHECK ADD  CONSTRAINT [FK_items] FOREIGN KEY([itemId])
REFERENCES [dbo].[Item] ([id])
GO
ALTER TABLE [dbo].[Item_Consignment] CHECK CONSTRAINT [FK_items]
GO
ALTER TABLE [dbo].[Item_Warehouse]  WITH CHECK ADD  CONSTRAINT [FK_itemsId] FOREIGN KEY([itemId])
REFERENCES [dbo].[Item] ([id])
GO
ALTER TABLE [dbo].[Item_Warehouse] CHECK CONSTRAINT [FK_itemsId]
GO
ALTER TABLE [dbo].[Item_Warehouse]  WITH CHECK ADD  CONSTRAINT [FK_warehouseId] FOREIGN KEY([warehouseId])
REFERENCES [dbo].[Warehouse] ([id])
GO
ALTER TABLE [dbo].[Item_Warehouse] CHECK CONSTRAINT [FK_warehouseId]
GO
/****** Object:  StoredProcedure [dbo].[Procedure]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Procedure]
	@param1 int = 0,
	@param2 int
AS
	SELECT @param1, @param2
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[spAddConsignment]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAddConsignment]
	@id int,
	@warehouseId int,
	@supplier varchar(MAX),
	@totalItems int,
	@arrivalDate datetime
AS
BEGIN
	INSERT INTO Consignment(id,warehouseId,supplier,totalItems,arrivalDate) values(@id, @warehouseId,@supplier,@totalItems,@arrivalDate)

END
GO
/****** Object:  StoredProcedure [dbo].[spAddItem_Consignment]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAddItem_Consignment]
	@itemId int,
	@consignmentId int,
	@quantity int,
	@expiry date
	
AS
BEGIN
	INSERT INTO Item_Consignment(itemId,consignmentId,quantity,expiry) values(@itemId, @consignmentId,@quantity,@expiry)

END
GO
/****** Object:  StoredProcedure [dbo].[spAddItems]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAddItems]
	@id int,
	@itemName varchar(MAX),
	@Manufacturer varchar(MAX),
	@Country varchar(MAX),
	@itemCode varChar(MAX)
AS
BEGIN
	INSERT INTO Item(id,itemName,Manufacturer,Country,itemCode) values(@id, @itemName,@Manufacturer,@Country,@itemCode)
END
GO
/****** Object:  StoredProcedure [dbo].[spAddShelf]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAddShelf]
	@zone varchar(MAX),
	@shelfName varchar(MAX),
	@shelfItems varchar(MAX),
	@slotsRemaining varchar(MAX),
	@warehouse_id int
AS
BEGIN
	Insert into Shelf(shelfName,warehouse_id,[zone],shelfItems,slotsRemaining) values(@shelfName,@warehouse_id,@zone,@shelfItems,@slotsRemaining) 
END
GO
/****** Object:  StoredProcedure [dbo].[spSelectWarehouse]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spSelectWarehouse]
	@managerId int
   
AS
BEGIN
    
    Select warehouseHTML FROM Warehouse
    WHERE managerId=@managerId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateConsignment]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateConsignment]
	@id int,
	@warehouseId int,
	@supplier varchar(MAX),
	@totalItems int,
	@arrivalDate datetime
AS
BEGIN
	Update Consignment SET warehouseId=@warehouseId, supplier=@supplier, totalItems=@totalItems , arrivalDate=@arrivalDate Where id=@id
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateItem_Consignment]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateItem_Consignment]
	@itemId int,
	@consignmentId int,
	@quantity int,
	@expiry date
	
AS
BEGIN
	Update Item_Consignment SET quantity=@quantity,expiry=@expiry Where(itemId=@itemId AND consignmentId=@consignmentId)

END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateShelf]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateShelf]
	@zone varchar(MAX),
	@shelfName varchar(MAX)
AS
BEGIN
	Update Shelf SET [zone]=@zone Where shelfName=@shelfName
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateStartWareHouse]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateStartWareHouse]
   
	@warehouseWidth float,
	@warehouseLength float,
	@scaledWarehouseLength float,
	@scaledWarehouseWidth float,
	@scaledShelfLength float,
	@scaledShelfWidth float,
	@shelfSlots int,

	@shelveLength float,
	@shelveHeight float,
	@shelveWidth float,
	@shelveRows float,
	@managerId int,
	@sections int
AS
BEGIN
  
    UPDATE Warehouse
    SET 
		
        ActualWarehouseWidth=@warehouseWidth,
		ActualWarehouseLength=@warehouseLength,
		actualShelfLength=@shelveLength,
		actualShelfWidth=@shelveWidth,
		actualShelfHeight=@shelveHeight,
		shelfRows=@shelveRows,
		scaledShelfLength=@scaledShelfLength,
		scaledShelfWidth=@scaledShelfWidth,
		scaledWarehouseLength=@scaledWarehouseLength,
		scaledWarehouseWidth=@scaledWarehouseWidth,
		shelfSlots=@shelfSlots,
		sections=@sections

    WHERE managerId=@managerId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateWareHouse]    Script Date: 1/11/2017 10:42:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateWareHouse]
    @warehouseHTML varbinary(MAX),
	@managerId int
AS
BEGIN
  
    UPDATE Warehouse
    SET warehouseHTML=@warehouseHTML
    WHERE managerId=@managerId
END
GO
USE [master]
GO
ALTER DATABASE [WarehouseDB] SET  READ_WRITE 
GO
