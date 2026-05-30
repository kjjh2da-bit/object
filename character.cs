using System;
using System.Collections.Generic;


public class OutOfSkillException : Exception
{
    public OutOfSkillException(string message) : base(message) { }
}


public class Skill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Damage { get; set; }

    public virtual int Use()
    {
        Console.WriteLine($"{Name} 스킬 사용! (데미지: {Damage})");
        return Damage;
    }
}
public class KnightSkill : Skill { }
public class MageSkill : Skill {  }


public class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int AttackPower { get; set; }
    

    public bool IsDead => Hp <= 0; 

    public virtual int Attack() { return AttackPower; }

   
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp < 0) Hp = 0;
        Console.WriteLine($"{Name}이(가) {damage}의 피해를 입었습니다! (남은 체력: {Hp}/{MaxHp})");
    }


    public void TakeDamage(int damage, string damageType)
    {
        Hp -= damage;
        if (Hp < 0) Hp = 0;
        Console.WriteLine($"{Name}이(가) {damage}의 [{damageType}] 피해를 입었습니다! (남은 체력: {Hp}/{MaxHp})");
    }
}


public class Player : Character
{
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Gold { get; set; }
    public int SkillUsesLeft { get; set; } 
    public List<Skill> MySkills { get; set; } = new List<Skill>();

   
    public void ShowSkills()
    {
        Console.WriteLine($"\n--- {Name}의 보유 스킬 ---");
        MySkills.ForEach(s => Console.WriteLine($"- {s.Name}: {s.Description}"));
    }

   
    public virtual int UseSkill(int skillNumber) 
    { 
        return 0; 
    }

    
    public virtual int UseSkill(string skillName) 
    { 
        return 0; 
    }
}


public class Knight : Player
{
    public Knight(string name)
    {
        Name = name; Level = 1; Exp = 0; Gold = 1000; SkillUsesLeft = 3;
        MaxHp = 150; Hp = 150; AttackPower = 20;
        MySkills.Add(new Skill { Name = "연속 베기", Description = "두 번 벱니다.", Damage = 40 });
        MySkills.Add(new Skill { Name = "방패 치기", Description = "방패로 찍습니다.", Damage = 25 });
    }

  
    public override int UseSkill(int skillNumber)
    {
        try
        {
           
            if (SkillUsesLeft <= 0) 
                throw new OutOfSkillException($"{Name}의 스킬 사용 횟수가 부족합니다!");

            int index = skillNumber - 1;
            SkillUsesLeft--;
            return MySkills[index].Use();
        }
        catch (OutOfSkillException e)
        {
            Console.WriteLine($"[알림] {e.Message}"); 
            return 0;
        }
    }

  
    public override int UseSkill(string skillName)
    {
        try
        {
            if (SkillUsesLeft <= 0) 
                throw new OutOfSkillException($"{Name}의 스킬 사용 횟수가 부족합니다!");

          
            Skill targetSkill = MySkills.Find(s => s.Name == skillName);
            
            if (targetSkill != null)
            {
                SkillUsesLeft--;
                return targetSkill.Use();
            }
            else
            {
                Console.WriteLine("그런 스킬은 배우지 않았습니다.");
                return 0;
            }
        }
        catch (OutOfSkillException e)
        {
            Console.WriteLine($"[알림] {e.Message}");
            return 0;
        }
    }
}
