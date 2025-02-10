using AutoMapper;
using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.DAL;
using Microsoft.EntityFrameworkCore.Metadata;
using GotExplorer.DAL.Entities;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GotExplorer.BLL.Services
{
    public class Model3DService : IModel3DService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _modelPath = "Models";
        private readonly IValidator<UploadModel3dDTO> _fileValidator;
        public Model3DService(AppDbContext appDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IValidator<UploadModel3dDTO> fileValidator)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _fileValidator = fileValidator;
        }

        public async Task<ValidationWithEntityModel<Model3dDTO>> GetModel3dAsync(int id)
        {
            var result = await _appDbContext.Models3D.FindAsync(id);

            if (result == null || !File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, result.Path)))
            {
                return new ValidationWithEntityModel<Model3dDTO>(
                    new ValidationFailure(nameof(id), ErrorMessages.ModelServiceModelNotFound, id) { ErrorCode = ErrorCodes.NotFound }
                );
            }
            var model = _mapper.Map<Model3dDTO>(result);
            return new ValidationWithEntityModel<Model3dDTO>(model);
        }

        public async Task<ValidationResult> UploadModel3dAsync(UploadModel3dDTO uploadModel3dDTO)
        {
            var validationResult = await _fileValidator.ValidateAsync(uploadModel3dDTO);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(uploadModel3dDTO.Model.FileName)}";
            var filePath = Path.Combine(_modelPath, fileName);
            var fileAbsolutePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fileAbsolutePath));
            using (var stream = new FileStream(fileAbsolutePath, FileMode.Create))
            {
                await uploadModel3dDTO.Model.CopyToAsync(stream);
            }

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                _appDbContext.Add(new Model3D()
                {
                    Name = uploadModel3dDTO.Model.FileName,
                    Path = filePath
                });
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                if (File.Exists(fileAbsolutePath))
                {
                    File.Delete(fileAbsolutePath);
                }

                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(uploadModel3dDTO.Model), ErrorMessages.ModelServiceFailedToUploadTheModel) { ErrorCode = ErrorCodes.Model3dUploadFailed },
                    ]);
            }
            return new ValidationResult();
        }

        public async Task<ValidationResult> UpdateModel3dAsync(int id, UploadModel3dDTO uploadModel3dDTO)
        {
            var validationResult = await _fileValidator.ValidateAsync(uploadModel3dDTO);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var model = await _appDbContext.Models3D.FindAsync(id);

            if (model == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.ModelServiceModelNotFound, id) { ErrorCode = ErrorCodes.NotFound },
                    ]);
            }

            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(uploadModel3dDTO.Model.FileName)}";
            var newFilePath = Path.Combine(_modelPath, newFileName);
            var newFileAbsolutePath = Path.Combine(_webHostEnvironment.WebRootPath, newFilePath);

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newFileAbsolutePath));
                using (var stream = new FileStream(newFileAbsolutePath, FileMode.Create))
                {
                    await uploadModel3dDTO.Model.CopyToAsync(stream);
                }
                var oldFileAbsolutePath = Path.Combine(_webHostEnvironment.WebRootPath, model.Path);

                model.Name = uploadModel3dDTO.Model.FileName;
                model.Path = newFilePath;
                _appDbContext.Models3D.Update(model);

                await _appDbContext.SaveChangesAsync();

                if (File.Exists(oldFileAbsolutePath))
                {
                    File.Delete(oldFileAbsolutePath);
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                if (File.Exists(newFileAbsolutePath))
                {
                    File.Delete(newFileAbsolutePath);
                }

                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.ModelServiceFailedToUpdateTheModel,id) { ErrorCode = ErrorCodes.Model3dUpdateFailed },
                    ]);
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> DeleteModel3dAsync(int id)
        {
            var model = await _appDbContext.Models3D.FindAsync(id);
            if (model == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.ModelServiceModelNotFound, id) { ErrorCode = ErrorCodes.NotFound }
                    ]);    
            }

            var fileAbsolutePath = Path.Combine(_webHostEnvironment.WebRootPath, model.Path);
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                _appDbContext.Models3D.Remove(model);
                await _appDbContext.SaveChangesAsync();

                if (File.Exists(fileAbsolutePath))
                {
                    File.Delete(fileAbsolutePath);
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(id), ErrorMessages.ModelServiceFailedToDeleteTheModel, id) { ErrorCode = ErrorCodes.Model3dDeletionFailed }
                    ]);
            }

            return new ValidationResult();
        }
    }
}
