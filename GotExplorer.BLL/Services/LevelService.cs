using AutoMapper;
using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.BLL.Validators;
using GotExplorer.DAL;
using GotExplorer.DAL.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
namespace GotExplorer.BLL.Services
{
    public class LevelService : ILevelService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IValidator<CreateLevelDTO> _createLevelValidator;
        private readonly IValidator<UpdateLevelDTO> _updateLevelValidator;
        private readonly IMapper _mapper;
        public LevelService(AppDbContext appDbContext, IValidator<CreateLevelDTO> createLevelValidator,IValidator<UpdateLevelDTO> updateLevelValidator, IMapper mapper) 
        {
            _appDbContext = appDbContext;
            _createLevelValidator = createLevelValidator;
            _updateLevelValidator = updateLevelValidator;
            _mapper = mapper;
        }

        public async Task<ValidationWithEntityModel<LevelDTO>> GetLevelAsync(int id)
        {
            var result = await _appDbContext.Levels.Include(x => x.Models).FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return new ValidationWithEntityModel<LevelDTO>(
                    new ValidationFailure(nameof(id), ErrorMessages.LevelServiceLevelNotFound, id) { ErrorCode = ErrorCodes.NotFound }
                );
                   
            }
            var level = _mapper.Map<LevelDTO>(result);
            return new ValidationWithEntityModel<LevelDTO>(level);
        }
        public async Task<ValidationResult> CreateLevelAsync(CreateLevelDTO levelDTO)
        {
            var validationResult = await _createLevelValidator.ValidateAsync(levelDTO);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var level = _mapper.Map<Level>(levelDTO);

                _appDbContext.AttachRange(level.Models);
                _appDbContext.Levels.Add(level);

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(levelDTO), ErrorMessages.LevelServiceFailedToCreateLevel,levelDTO) { ErrorCode = ErrorCodes.LevelCreationFailed },
                    ]);
            }
            return new ValidationResult();
        }
        public async Task<ValidationResult> UpdateLevelAsync(int id, UpdateLevelDTO levelDTO)
        {
            var validationResult = await _updateLevelValidator.ValidateAsync(levelDTO);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }
            var level = await _appDbContext.Levels.Include(x => x.Models).FirstOrDefaultAsync(x => x.Id == id);

            if (level == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.LevelServiceLevelNotFound,id) { ErrorCode = ErrorCodes.NotFound },
                    ]);
            }
            var models = level.Models.ToList();
          

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                level.Models.Clear();
                await _appDbContext.SaveChangesAsync();
                _appDbContext.ChangeTracker.Clear();

                _mapper.Map(levelDTO, level);
                
                _appDbContext.AttachRange(level.Models);
                _appDbContext.Levels.Update(level);
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.LevelServiceFailedToUpdateLevel, id) { ErrorCode = ErrorCodes.LevelUpdateFailed },
                    ]);
            }
            return new ValidationResult();
        }
        public async Task<ValidationResult> DeleteLevelAsync(int id)
        {
            var level = await _appDbContext.Levels.FindAsync(id);
            if (level == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.LevelServiceLevelNotFound,id) { ErrorCode = ErrorCodes.NotFound },
                    ]);
            }

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                _appDbContext.Levels.Remove(level);
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.LevelServiceFailedToDeleteLevel,id) { ErrorCode = ErrorCodes.LevelDeletionFailed },
                    ]);
            }

            return new ValidationResult();
        }
    }
}
