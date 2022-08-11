using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerKing.Api.Domain.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "E-mail é campo obrigatório para Login.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é campo obrigatório para Login.")]
        [StringLength(255, ErrorMessage = "Senha deve ter no máximo {1} caracteres.")]
        public string Password { get; set; }
    }
}
