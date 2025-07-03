using AutoMapper;
using PresupuestoFamiliarMensual.Application.DTOs;
using PresupuestoFamiliarMensual.Core.Entities;

namespace PresupuestoFamiliarMensual.Application.Mapping;

/// <summary>
/// Perfil de mapeo de AutoMapper
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de Budget
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.FamilyMemberName, opt => opt.MapFrom(src => src.FamilyMember.Name))
            .ForMember(dest => dest.MonthName, opt => opt.MapFrom(src => src.Month.Name))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Month.Year))
            .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.TotalSpent))
            .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => src.RemainingAmount))
            .ForMember(dest => dest.IsOverBudget, opt => opt.MapFrom(src => src.IsOverBudget))
            .ForMember(dest => dest.CategoryCount, opt => opt.MapFrom(src => src.Categories.Count))
            .ForMember(dest => dest.ExpenseCount, opt => opt.MapFrom(src => src.Expenses.Count));

        // Mapeo de BudgetCategory
        CreateMap<BudgetCategory, BudgetCategoryDto>()
            .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.TotalSpent))
            .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => src.RemainingAmount))
            .ForMember(dest => dest.IsOverLimit, opt => opt.MapFrom(src => src.IsOverLimit))
            .ForMember(dest => dest.ExpenseCount, opt => opt.MapFrom(src => src.Expenses.Count));

        // Mapeo de Expense
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.BudgetCategory.Name))
            .ForMember(dest => dest.FamilyMemberName, opt => opt.MapFrom(src => src.FamilyMember.Name));
    }
} 