using JarvisAI.Domain.Interfaces;
using JarvisAI.Domain.Entities;
using JarvisAI.Infraestructure.Data;

namespace JarvisAI.Infraestructure.Tools;

public class ReminderTool : ITool
{
    public string Name => "ReminderTool";
    public string Description => "Gerencia lembretes e tarefas. Input: 'criar: texto da tarefa' ou 'listar' ou 'concluir: id da tarefa'";

    private readonly JarvisDbContext _db;

    public ReminderTool(JarvisDbContext db)
    {
        _db = db;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        try
        {
            var command = input.ToLower().Trim();

            if (command.StartsWith("criar:"))
            {
                var text = input.Substring(6).Trim();
                _db.Reminders.Add(new Reminder
                {
                    Text = text,
                    Done = false,
                    CreatedAt = DateTime.UtcNow
                });
                await _db.SaveChangesAsync();
                return $"Lembrete criado: '{text}'";
            }

            if (command.StartsWith("concluir:"))
            {
                var id = int.Parse(input.Substring(9).Trim());
                var reminder = await _db.Reminders.FindAsync(id);
                if (reminder is null) return "Lembrete não encontrado.";
                reminder.Done = true;
                await _db.SaveChangesAsync();
                return $"Lembrete '{reminder.Text}' concluído!";
            }

            if (command == "listar")
            {
                var reminders = _db.Reminders
                    .Where(r => !r.Done)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToList();

                if (!reminders.Any()) return "Nenhum lembrete pendente.";

                var result = "Seus lembretes pendentes:\n";
                foreach (var r in reminders)
                    result += $"• [{r.Id}] {r.Text}\n";

                return result;
            }

            return "Comando inválido. Use: 'criar: tarefa', 'listar' ou 'concluir: id'";
        }
        catch
        {
            return "Erro ao gerenciar lembretes.";
        }
    }
}