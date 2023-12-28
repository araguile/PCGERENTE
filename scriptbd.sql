USE [dbprueba]
GO
/****** Object:  Table [dbo].[factura]    Script Date: 27/12/2023 20:06:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[factura](
	[idFactura] [int] IDENTITY(1,1) NOT NULL,
	[fecha] [date] NOT NULL,
	[total] [money] NOT NULL,
 CONSTRAINT [PK_factura] PRIMARY KEY CLUSTERED 
(
	[idFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[facturaDetalle]    Script Date: 27/12/2023 20:06:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[facturaDetalle](
	[idDetalle] [int] IDENTITY(1,1) NOT NULL,
	[idFactura] [int] NOT NULL,
	[idProducto] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
	[iva] [money] NOT NULL,
	[subtotal] [money] NOT NULL,
 CONSTRAINT [PK_facturaDetalle] PRIMARY KEY CLUSTERED 
(
	[idDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[facturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_facturaDetalle_factura] FOREIGN KEY([idFactura])
REFERENCES [dbo].[factura] ([idFactura])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[facturaDetalle] CHECK CONSTRAINT [FK_facturaDetalle_factura]
GO

create proc spconsultasecuencial
@idfactura int output
as
	set @idfactura= IDENT_CURRENT ('factura') + 1 
go

CREATE proc [dbo].[spmostrar_factura]
as
	select idfactura,
			fecha,
			total
	from factura fa
go

create proc [dbo].[spinsertar_factura]
@idfactura int output,
@fecha datetime,
@total money
as
	insert into factura(fecha,total)
	values(@fecha,@total)

	set @idfactura=@@IDENTITY
go

CREATE proc [dbo].[spinsertar_detalle_factura]
@idventa int,
@idproducto int,
@cantidad int,
@subtotal money,
@iva money
as
	insert into facturadetalle(idfactura,idproducto,cantidad,subtotal,iva)
	values(@idventa,@idproducto,@cantidad,@subtotal,@iva)
go