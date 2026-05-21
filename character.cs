using System;

// 1. 최상위 부모 클래스 (모든 캐릭터의 뼈대)
public class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int AttackPower { get; set; }
    
    // 체력이 0 이하면 죽은 것으로 판정
    public bool IsDead => Hp <= 0; 

    // 공격 (자식 클래스에서 오버라이딩 할 예정)
    public virtual int Attack()
    {
        return AttackPower;
    }

    // 데미지를 입는 기능
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp < 0) Hp = 0; // 체력이 마이너스가 되지 않게 처리
        Console.WriteLine($"{Name}이(가) {damage}의 피해를 입었습니다! (남은 체력: {Hp}/{MaxHp})");
    }
}

// 2. 플레이어 클래스 (Character 상속)
public class Player : Character
{
    // [기획서 반영] 플레이어만 가지는 속성: 레벨, 경험치, 골드, 스킬 사용 횟수
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Gold { get; set; }
    public int SkillUsesLeft { get; set; } 

    public Player(string name)
    {
        Name = name;
        Level = 1;
        Exp = 0;
        Gold = 1000;       // 초기 자금
        SkillUsesLeft = 3; // 스킬 사용 가능 횟수 (예시로 3번 제한)
    }

    // [기획서 반영] 행동 2: 도망가기 (일정 확률로 실패)
    public bool RunAway()
    {
        Random rand = new Random();
        int chance = rand.Next(1, 101); // 1~100 랜덤 뽑기

        if (chance > 50) // 50% 확률 성공
        {
            Console.WriteLine($"{Name}이(가) 무사히 도망쳤습니다!");
            return true;
        }
        else // 50% 확률 실패
        {
            Console.WriteLine($"{Name}이(가) 도망치는 데 실패했습니다!");
            return false;
        }
    }

    // [기획서 반영] 행동 3: 아이템 사용 (3번 팀원이 만들 Item과 연결될 기본 틀)
    public void UseItem()
    {
        Console.WriteLine($"{Name}이(가) 가방에서 아이템을 꺼내 사용합니다!");
    }

    // 스킬 사용 기본 틀
    public virtual void UseSkill(int skillNumber)
    {
        Console.WriteLine("기본 스킬이 없습니다.");
    }
}

// 3. 기사 클래스 (Player 상속)
public class Knight : Player
{
    public Knight(string name) : base(name)
    {
        MaxHp = 150;
        Hp = 150;
        AttackPower = 20;
    }

    // [기획서 반영] 행동 1: 기본 공격 오버라이딩
    public override int Attack()
    {
        Console.WriteLine($"[기사] {Name}의 묵직한 검 휘두르기!");
        return AttackPower;
    }

    // [기획서 반영] 직업별 스킬 2개 및 횟수 제한 오버라이딩
    public override void UseSkill(int skillNumber)
    {
        if (SkillUsesLeft <= 0)
        {
            Console.WriteLine("스킬 사용 횟수를 모두 소모했습니다!");
            return;
        }

        SkillUsesLeft--; // 스킬 쓸 때마다 횟수 차감
        
        if (skillNumber == 1) Console.WriteLine($"[스킬1] 연속 베기! (남은 횟수: {SkillUsesLeft})");
        else if (skillNumber == 2) Console.WriteLine($"[스킬2] 방패 치기! (남은 횟수: {SkillUsesLeft})");
        else { Console.WriteLine("잘못된 스킬 번호입니다."); SkillUsesLeft++; }
    }
}

// 4. 마법사 클래스 (Player 상속)
public class Mage : Player
{
    public Mage(string name) : base(name)
    {
        MaxHp = 80;
        Hp = 80;
        AttackPower = 30;
    }

    // [기획서 반영] 행동 1: 기본 공격 오버라이딩
    public override int Attack()
    {
        Console.WriteLine($"[마법사] {Name}의 화염 지팡이 공격!");
        return AttackPower;
    }

    // [기획서 반영] 직업별 스킬 2개 및 횟수 제한 오버라이딩
    public override void UseSkill(int skillNumber)
    {
        if (SkillUsesLeft <= 0)
        {
            Console.WriteLine("스킬 사용 횟수를 모두 소모했습니다!");
            return;
        }

        SkillUsesLeft--;
        
        if (skillNumber == 1) Console.WriteLine($"[스킬1] 파이어볼! (남은 횟수: {SkillUsesLeft})");
        else if (skillNumber == 2) Console.WriteLine($"[스킬2] 얼음 창! (남은 횟수: {SkillUsesLeft})");
        else { Console.WriteLine("잘못된 스킬 번호입니다."); SkillUsesLeft++; }
    }
}
