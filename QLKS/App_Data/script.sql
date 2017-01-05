 
ALTER DATABASE [QLNS] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLNS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLNS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLNS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLNS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLNS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLNS] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLNS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [QLNS] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [QLNS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLNS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLNS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLNS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLNS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLNS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLNS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLNS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLNS] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QLNS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLNS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLNS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLNS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLNS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLNS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLNS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLNS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QLNS] SET  MULTI_USER 
GO
ALTER DATABASE [QLNS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLNS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLNS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLNS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
/****** Object:  Table [dbo].[Amenity]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Amenity](
	[AmenityID] [nvarchar](15) NOT NULL,
	[AmenityName] [nvarchar](50) NULL,
	[Price] [money] NULL,
	[AmenityUnit] [nvarchar](50) NULL,
	[MiniBar] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AmenityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckIn](
	[CheckInID] [int] NOT NULL,
	[GroupID] [int] NULL,
	[RoomNoID] [nvarchar](15) NULL,
	[DateIn] [datetime] NULL,
	[Discount] [int] NULL,
	[Tax] [money] NULL,
	[DateOut] [datetime] NULL,
	[OutofTime] [int] NULL,
	[Comment] [nvarchar](100) NULL,
	[CustomerID] [int] NULL,
	[RoomCharge] [money] NULL,
	[CheckInStatus] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CheckInID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CONFIGCOMP]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CONFIGCOMP](
	[CompanyName] [nvarchar](250) NOT NULL,
	[CompanyAdd] [nvarchar](250) NULL,
	[TaxCode] [char](15) NULL,
	[AccountNo] [char](15) NULL,
	[Phone] [char](15) NULL,
	[Fax] [int] NULL,
	[FloorNo] [int] NULL,
	[PositionNo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] NOT NULL,
	[IDCard] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Mail] [nvarchar](50) NULL,
	[Address] [nvarchar](100) NULL,
	[Sex] [bit] NULL,
	[NationalityID] [int] NULL,
	[Company] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[AmenityID] [nvarchar](15) NOT NULL,
	[RoomTypeID] [nvarchar](15) NULL,
	[Amount] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Groups]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupID] [int] NOT NULL,
	[GroupName] [nvarchar](100) NULL,
	[CustomerID] [int] NULL,
	[DateIn] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[InvoiceID] [int] NOT NULL,
	[InvoiceDate] [date] NULL,
	[CheckInID] [int] NULL,
	[Total] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Nationality]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nationality](
	[NationalityID] [int] NOT NULL,
	[NationalityName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[NationalityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RevervationRoom]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RevervationRoom](
	[ReservationID] [int] NOT NULL,
	[RoomTypeID] [nvarchar](15) NULL,
	[Numb] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReservationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Revervations]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Revervations](
	[ReservationID] [int] NOT NULL,
	[IsGroup] [bit] NULL,
	[GroupName] [nvarchar](100) NULL,
	[CustomerID] [int] NULL,
	[PeopleNumber] [int] NULL,
	[ListRoom] [nvarchar](100) NULL,
	[DateIn] [datetime] NULL,
	[DateOut] [datetime] NULL,
	[Requests] [nvarchar](100) NULL,
	[Status] [int] NULL,
	[BookingDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReservationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Room]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[RoomNoID] [nvarchar](15) NOT NULL,
	[Floor] [int] NULL,
	[Position] [int] NULL,
	[RoomTypeID] [nvarchar](15) NULL,
	[RoomStatus] [int] NULL,
	[RoomPhone] [nvarchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomNoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoomType]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomType](
	[RoomTypeID] [nvarchar](15) NOT NULL,
	[RoomTypeName] [nvarchar](50) NULL,
	[Price] [money] NULL,
	[Depcriptions] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceType](
	[ServiceID] [nvarchar](50) NOT NULL,
	[ServiceName] [nvarchar](50) NULL,
	[ServiceUnit] [nchar](10) NULL,
	[Price] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UseAmenity]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UseAmenity](
	[AmentityID] [nvarchar](15) NOT NULL,
	[CheckInID] [int] NULL,
	[NumberUse] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsedService]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsedService](
	[ServiceID] [nvarchar](50) NOT NULL,
	[UsedServiceCodeID] [int] NULL,
	[NumberUse] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsedServiceCode]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsedServiceCode](
	[UsedServiceCodeID] [int] NOT NULL,
	[CheckInID] [int] NULL,
	[GuestNameUsed] [nvarchar](50) NULL,
	[ServiceDate] [date] NULL,
	[DateUsed] [datetime] NULL,
	[ServiceComment] [nvarchar](100) NULL,
	[StatusUsed] [bit] NULL,
	[Payed] [money] NULL,
	[Total] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[UsedServiceCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/14/2016 6:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UsersID] [nvarchar](30) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Birth] [date] NULL,
	[NativeCountry] [nvarchar](50) NULL,
	[Sex] [bit] NULL,
	[systemRolle] [bit] NULL,
	[resourceRolle] [bit] NULL,
	[ReservationRolle] [bit] NULL,
	[reportRolle] [bit] NULL,
	[Pass] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[UsersID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_Customers]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_Groups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_Groups]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_Room] FOREIGN KEY([RoomNoID])
REFERENCES [dbo].[Room] ([RoomNoID])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_Room]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Nationality] FOREIGN KEY([NationalityID])
REFERENCES [dbo].[Nationality] ([NationalityID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Nationality]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_Amenity] FOREIGN KEY([AmenityID])
REFERENCES [dbo].[Amenity] ([AmenityID])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [FK_Equipment_Amenity]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_RoomType] FOREIGN KEY([RoomTypeID])
REFERENCES [dbo].[RoomType] ([RoomTypeID])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [FK_Equipment_RoomType]
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Customers]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_CheckIn] FOREIGN KEY([CheckInID])
REFERENCES [dbo].[CheckIn] ([CheckInID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_CheckIn]
GO
ALTER TABLE [dbo].[RevervationRoom]  WITH CHECK ADD  CONSTRAINT [FK_RevervationRoom_Revervations] FOREIGN KEY([ReservationID])
REFERENCES [dbo].[Revervations] ([ReservationID])
GO
ALTER TABLE [dbo].[RevervationRoom] CHECK CONSTRAINT [FK_RevervationRoom_Revervations]
GO
ALTER TABLE [dbo].[RevervationRoom]  WITH CHECK ADD  CONSTRAINT [FK_RevervationRoom_RoomType] FOREIGN KEY([RoomTypeID])
REFERENCES [dbo].[RoomType] ([RoomTypeID])
GO
ALTER TABLE [dbo].[RevervationRoom] CHECK CONSTRAINT [FK_RevervationRoom_RoomType]
GO
ALTER TABLE [dbo].[Revervations]  WITH CHECK ADD  CONSTRAINT [FK_Revervations_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Revervations] CHECK CONSTRAINT [FK_Revervations_Customers]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_RoomType] FOREIGN KEY([RoomTypeID])
REFERENCES [dbo].[RoomType] ([RoomTypeID])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_RoomType]
GO
ALTER TABLE [dbo].[ServiceType]  WITH CHECK ADD  CONSTRAINT [FK_ServiceType_UsedService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[UsedService] ([ServiceID])
GO
ALTER TABLE [dbo].[ServiceType] CHECK CONSTRAINT [FK_ServiceType_UsedService]
GO
ALTER TABLE [dbo].[UseAmenity]  WITH CHECK ADD  CONSTRAINT [FK_UseAmenity_Amenity] FOREIGN KEY([AmentityID])
REFERENCES [dbo].[Amenity] ([AmenityID])
GO
ALTER TABLE [dbo].[UseAmenity] CHECK CONSTRAINT [FK_UseAmenity_Amenity]
GO
ALTER TABLE [dbo].[UsedService]  WITH CHECK ADD  CONSTRAINT [FK_UsedService_UsedServiceCode] FOREIGN KEY([UsedServiceCodeID])
REFERENCES [dbo].[UsedServiceCode] ([UsedServiceCodeID])
GO
ALTER TABLE [dbo].[UsedService] CHECK CONSTRAINT [FK_UsedService_UsedServiceCode]
GO
ALTER TABLE [dbo].[UsedServiceCode]  WITH CHECK ADD  CONSTRAINT [FK_UsedServiceCode_CheckIn] FOREIGN KEY([CheckInID])
REFERENCES [dbo].[CheckIn] ([CheckInID])
GO
ALTER TABLE [dbo].[UsedServiceCode] CHECK CONSTRAINT [FK_UsedServiceCode_CheckIn]
GO
ALTER DATABASE [QLNS] SET  READ_WRITE 
GO


CREATE SEQUENCE Amenity_seq
  AS BIGINT
  START WITH 1
  INCREMENT BY 1
  MINVALUE 1
  MAXVALUE 9999999999
  NO CYCLE
  CACHE 10;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         ��                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         �$g             @                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            X�30                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         �g`                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       