#pragma once

#include "EngineTypes.h"
#include "ActionParams.h"
#include "EngineGameObject.h"
#include "EngineGame.h"
#include "ActionEffect.h"
#include "ActionCondition.h"
#include "DamageTypes.h"
#include "FormationDefines.h"
#include "RPGGame.h"

#define MAX_NUM_TARGETS 32
#define NUM_STATUS_EFFECTS_PER_ACTION 10

using namespace Engine;

namespace Engine
{
	class ScriptedScene;
}

class BaseEntity;
class BaseEntityHandle;

struct ActionInitData;
class Action;

extern bool g_bCanActionsMiss;
extern bool g_bCanActionsTargetNext;
extern float g_fDefaultActionAccuracy;
extern float g_fDefaultDamageAndHealVariance;

enum ActionClass
{
	ACTIONTYPE_DEFAULT = 0,
	ACTIONTYPE_ATTACK,
	ACTIONTYPE_SUPPORT,
	ACTIONTYPE_RANDOM,
	ACTIONTYPE_STEALITEM,
	ACTIONTYPE_USEITEM,
	ACTIONTYPE_SWAPATTRIBUTE,
	ACTIONTYPE_AVERAGESTATS,
	ACTIONTYPE_BATTLEBONUS,
	ACTIONTYPE_SEQUENCE,
	ACTIONTYPE_CREATEENTITY,
	ACTIONTYPE_SELFDESTRUCT,
	ACTIONTYPE_MODIFYLAST,
	ACTIONTYPE_FLEE,
	ACTIONTYPE_GUI,
	ACTIONTYPE_GIVEITEM,
	ACTIONTYPE_GAMEFLAG,
	ACTIONTYPE_AUTOKILL,
	ACTIONTYPE_ENABLEDEATHACTION,
	ACTIONTYPE_CHANGETARGET,
	ACTIONTYPE_MULTITURN,
	ACTIONTYPE_DELAYED,
	ACTIONTYPE_REACT,
	NUM_ACTION_CLASSES
};

struct ActionComboOwnerInitData : public ObjectInitData
{
	ActionComboOwnerInitData();

	PIPELINE_CHARACTER_REQUIRED(Character, -1, true);
	ActionComboOwnerInitData* pNext;		// Linked list of all owners
};

class ActionComboOwner : public Object
{
	REGISTER_CLASS_DECL(ActionComboOwner, "ActionComboOwner", Object)
public:
	ActionComboOwner(ObjectInitData *pInitData);
	CharacterReference m_Owner;

	ActionComboOwner* next;
};

struct SubActionInitData : public ObjectInitData
{
	SubActionInitData();
	virtual ~SubActionInitData();

#ifdef PIPELINE_TOOL
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
#else
	virtual void ReadFromFile(FileCollection* saveFile);
#endif

	PIPELINE_FLOAT(ActivationChance, 1.0f);
	PIPELINE_FLOAT(ActivationChancePerAdditionalTarget, 0.0f);
	PIPELINE_BOOL(PostAction, true);
	PIPELINE_BOOL(ManualExecution, false);
	PIPELINE_BOOL(CanSubActionMiss, false);

	SubActionInitData* pNext;		// Linked list of all our actions to be initialized
	ActionInitData* m_pAction;
};

/**
*  Action that has a chance of being activated after the execution of another Action
*/
class SubAction : public Object
{
	REGISTER_CLASS_DECL(SubAction, "SubAction", Object)
public:
	SubAction(ObjectInitData *pInitData);
	virtual ~SubAction();
	float GetActivationChance();
	inline bool IsPostAction() { return m_bPostAction; }
	inline bool CanExecute() { return !m_bManuallyExecuted; }
	inline Action* GetAction() { return m_pAction; }
	virtual void Destroy();
	void IncrementNumAttempts() { m_iNumAttempts++; }
	void IncrementNumChosen() { m_iNumChosen++; }
	void SetActivationChance(float fChance) { m_fActivationChance = fChance; }

	bool RequiresExecution();
	bool CanPassRandomization();
	void SetActionOwner(Action* pOwner);

	SubAction* next;

private:
	float m_fActivationChance; /**< Base chance of this SubAction activating \n Example: ActivationChance="0.5" \n Default: 1.0 */
	float m_fActivationChancePerAdditionalTarget; /**< Bonus chance of this SubAction activating \n per target beyond the first \n Example: ActivationChancePerAdditionalTarget="0.5" \n Default: 0.0 */
	PLAT_BOOL m_bPostAction; /**< Flag indicating this action activates after the base action \n Example: PostAction="False" \n Default: True */
	PLAT_BOOL m_bManuallyExecuted;  /**< Flag indicating this sub action is manually executed by its owner, not as part of the base Action resolution \n Example: ManualExecution="True" \n Default: False */
	Action* m_pAction;

	int m_iNumChosen;
	int m_iNumAttempts;
};

struct ActionInitData : public GameObjectInitData
{
	ActionInitData();
	virtual ~ActionInitData();
	void Copy(const ObjectInitData& object);

#ifdef PIPELINE_TOOL
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
#else
	virtual void ReadFromFile(FileCollection* saveFile);
#endif

	PIPELINE_ENUM(ActionType, g_szActionTypes, NUM_ACTION_TYPES, ActionAny);
	PIPELINE_CHARACTER(ActionOwner, -1);
	PIPELINE_ENUM_FUNCTION(ActionOwnerType, ActionTargetAny, &GameEngine::GetActionTargetTypeByName);
	PIPELINE_ENUM_FUNCTION(ActionTargetType, ActionTargetAny, &GameEngine::GetActionTargetTypeByName);
	PIPELINE_CHARACTER(ActionTarget, -1);
	PIPELINE_ENUM(ActionTargetState, g_szActionTargetStates, NUM_ACTION_TARGET_STATES, ActionTargetStateAny);
	PIPELINE_FLOAT(Cooldown, 0.0f); /**< Cooldown after this action is used \n Example: Cooldown="1.25" \n Default: 1.0 */
	PIPELINE_FLOAT(PrepTime, 0.0f); /**< How long does this action take to be prepared after it is selected \n Example: PrepTime="1.0" \n Default: 0.0 */
	PIPELINE_FLOAT(TurnCost, 1.0f); /**< For systems where an action may cost more or less than 1 full turn \n Example: TurnCost="0.5" \n Default: 1.0 */
	PIPELINE_INT(Weight, 100); /**< Weight value for the AI's random selection \n Example: Weight="200" \n Default: 100 */
	PIPELINE_INT(WeightIncreasePerTurn, 0); /**< Weight value increase per turn for the AI's random selection \n Example: WeightIncreasePerTurn="10" \n Default: 0 */
	PIPELINE_INT(WeightChangeAfterUse, 0); /**< Weight value change after use for the AI's random selection \n Example: WeightChangeAfterUse="10" \n Default: 0 */
	PIPELINE_INT(MinWeight, 0); /**< Min weight this action can ever have after after modifications \n Example: MinWeight="10" \n Default: 0 */
	PIPELINE_INT(MaxWeight, 100000000); /**< Max weight this action can ever have after after modifications \n Example: MaxWeight="1000" \n Default: 100000000 */
	PIPELINE_FLOAT(RandomFactor, g_fDefaultDamageAndHealVariance);
	PIPELINE_STRINGHASH(GotoActionList, 0); /**< Name of Action List for the AI to select after this action is used \n Example: GotoActionList="MyOtherList" \n Default: None */
	PIPELINE_STRINGHASH(TargetGotoActionList, 0); /**< Name of Action List for the target to select after this action is used \n Example: TargetGotoActionList="MyOtherList" \n Default: None */
	PIPELINE_STRINGHASH(Message, 0); /**< Name of the message to send to the target \n Example: Message="NotifySomething" \n Default: None */
	PIPELINE_STRINGHASH(Anim, ToStringHash("Attack")); /**< Animation to play for this action.\nExample: Anim="Dodge" \n Default: Attack */
	PIPELINE_STRINGHASH(FinishAnim, 0); /**< Animation to play for this action when it finishes.\nExample: Anim="Dodge" \n Default: None */
	PIPELINE_LOCID(UseText, 0); /**< Localization string ID to display upon the use of this action \n Example: UseText="LOC_ATTACK" \n Default: None */
	PIPELINE_LOCID(TargetText, 0); /**< Localization string ID to display for the target of this action \n Example: TargetText="LOC_POISON" \n Default: None */
	PIPELINE_LOCID(UseSelfText, 0); /**< Localization string ID to display when the target of this action is the owner \n Example: TargetText="LOC_POISON" \n Default: None */
	PIPELINE_LOCID(DescriptionText, 0); /**< Localization string ID to display for the description of this action \n Example: DescriptionText="LOC_ATTACK_DESCRIPTION" \n Default: None */
	PIPELINE_LOCID(MissText, 0); /**< Localization string ID to display for when this action misses \n Example: DescriptionText="LOC_FAILURE" \n Default: LOC_MISS_MESSAGE */
	PIPELINE_LOCID(FailText, 0); /**< Localization string ID to display for when this action fails to execute successfully \n Example: FailText="LOC_FAILURE" \n Default: LOC_ACTION_FAIL */
	PIPELINE_LOCID(FinishText, 0); /**< Localization string ID to display when this action finishes \n Example: FinishText="LOC_FINISH" \n Default: None */
	PIPELINE_INT(Level, 1); /**< Level requirement to use this action \n Example: Level="5" \n Default: 1 */
	PIPELINE_INT(Citizen, -1); /**< Level requirement to use this action \n Example: Level="5" \n Default: 1 */
	PIPELINE_INT(JobLevel, 1); /**< Job Level requirement to use this action \n Example: Level="5" \n Default: 1 */
	PIPELINE_INT(MPCost, 0); /**< MP cost of this action \n Example: MPCost="5" \n Default: 0 */
	PIPELINE_FLOAT(HPCostPercent, 0.0f); /**< Percentage HP cost of this action \n Example: HPCostPercent="0.1" \n Default: 0 */
	PIPELINE_ENUM(DamageType, g_szDamageTypeNames, NUM_DAMAGE_TYPES, DamageType_Melee); /**< Type of damage. Informs what stat to use in the damage equation \n Example: DamageType="Melee" \n Default: Melee \n Options: Melee, Magic*/
	PIPELINE_ENUM(ElementDamageType, g_szElementDamageTypeNames, NUM_ELEMENT_DAMAGE_TYPES, ElementDamageType_None); /**< Type of elemental damage that informs what kind of elemental defense is applied against this action \n Example: ElementDamageType="Fire" \n Default: None \n Options: None, Fire, Ice, Lightning */
	PIPELINE_BOOL(Silent, false); /**< Flag indicating if this action should execute without displaying text \n Example: Silent="True" \n Default: False */
	PIPELINE_BOOL(Overworld, false); /**< Flag indicating whether this action can be used on the overworld (i.e. outside of battle) \n Example: Overworld="True" \n Default: False */
	PIPELINE_BOOL(GlobalAccuracy, false); /**< Flag indicating whether the accuracy test is done once globally for all targets, rather than once per target \n Example: GlobalAccuracy="False" \n Default: False */
	PIPELINE_BOOL(CanMiss, true); /**< Flag indicating whether this action can miss \n Example: CanMiss="False" \n Default: True */
	PIPELINE_BOOL(CanTargetDead, false); /**< Flag indicating whether this action can target the dead \n Example: CanTargetDead="True" \n Default: False */
	PIPELINE_BOOL(CanTargetNext, true); /**< Flag indicating whether this action can target the next target if the proper target is dead \n Example: CanTargetNext="False" \n Default: True */
	PIPELINE_BOOL(FailSilently, false); /**< Flag indicating whether this action should fail without printing text \n Example: FailSilently="True" \n Default: False */
	PIPELINE_BOOL(NoShowOwnerInUseText, false); /**< Flag indicating whether the owner should be ignored in the use text \n Example: NoShowOwnerInUseText="True" \n Default: False */
	PIPELINE_BOOL(EnsureValidTarget, false); /**< Flag indicating that the AI will ensure the action has a valid target before selecting it \n Example: EnsureValidTarget="True" \n Default: False */
	PIPELINE_BOOL(OnlyChooseIfWillHaveEffect, false); /**< Flag indicating that the AI will only choose this action if it will actually have an effect \n Example: OnlyChooseIfWillHaveEffect="True" \n Default: False */
	PIPELINE_BOOL(ExecuteOnlyIfOwnerDead, false); /**< Flag indicating that action will only execute even if the owner is dead and not otherwise \n Example: ExecuteOnlyIfOwnerDead="True" \n Default: False */
	PIPELINE_BOOL(ExecuteIfOwnerDead, false); /**< Flag indicating that action will still execute even if the owner is dead \n Example: ExecuteIfOwnerDead="True" \n Default: False */
	PIPELINE_FLOAT(SpeedModifier, 1.0f); /**< Game-dependent speed value that will modify action speed value to influence turn order (higher valued actions go before lower valued) \n Example: SpeedModifier="2" \n Default: 1.0 */
	PIPELINE_FLOAT(AccuracyModifier, g_fDefaultActionAccuracy); /**< Multiply owner's accuracy value by this for accuracy calculation \n Example: AccuracyModifier="2.0" \n Default: 0.95 */

	PIPELINE_FLOAT(AggroChance, 0.0f); /**< Chance to draw aggro from target \n Example: AggroChance="0.5" \n Default: 0.0 \n If value is less than 0.0, the chance is used to reduce aggro, not draw it*/
	PIPELINE_FLOAT(AggroPersistChance, 0.0f); /**< Chance for aggro from this action to persist from turn to turn \n Example: AggroPersistChance="0.75" \n Default: 1.0 */

	PIPELINE_FLOAT(TargetHealthMultiplier, 0.0f); /**< Multiply by target health% to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of target health. \n Example: TargetHealthMultiplier="2.0" \n Default: 0.0 */
	PIPELINE_FLOAT(UserHealthMultiplier, 0.0f); /**< Multiply by user health% to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of target health. \n Example: UserHealthMultiplier="2.0" \n Default: 0.0 */
	PIPELINE_FLOAT(UserMPMultiplier, 0.0f); /**< Multiply by user MP% to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of target health. \n Example: UserHealthMultiplier="2.0" \n Default: 0.0 */
	PIPELINE_FLOAT(ExemptFromMultipliers, 0.0f); /**< Portion of action effectiveness to not receive any multipliers \n Example: ExemptFromMultipliers="0.5" \n Default: 0.0 */

	PIPELINE_FLOAT(TargetDamageReceivedMultiplier, 0.0f); /**< Multiply by damage received this turn to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of damage. \n Example: TargetDamageReceivedMultiplier="2" \n Default: 0 */
	PIPELINE_FLOAT(ConsecutiveUseMultiplier, 1.0f); /**< Multiply by the number of consecutive uses of this action to generate action effectiveness (as determined by derived actions). \n Example: ConsecutiveUseMultiplier="2" \n Default: 1.0 */
	PIPELINE_INT(MaxConsecutiveUseBoost, 1); /**< The maximum number of consecutive uses to use with the ConsecutiveUseMultiplier value. \n Example: MaxConsecutiveUseBoost="4" \n Default: 0 */

	PIPELINE_BOOL(UsesExecutionPosition, false);
	PIPELINE_BOOL(CanBeCountered, true);

	PIPELINE_ENUM(ActionDisplayType, g_szActionDisplayTypes, NUM_ACTION_DISPLAY_TYPES, ActionDisplayNormal); /**< Type of display for this action. \n Example: ActionDisplayType="Binary" \n Default: Normal \n Options: Normal, Hex, Binary */

	ActionEffectSetInitData* m_pActionEffectSetInitData;

	ActionInitData* pNext;		// Linked list of all our actions to be initialized

	PIPELINE_OBJECTS(ActionConditions, ActionConditionInitData, ActionCondition);

	PIPELINE_OBJECTS(pActionComboOwners, ActionComboOwnerInitData, ActionComboOwner);

	PIPELINE_OBJECTS(SubActions, SubActionInitData, SubAction);
	PIPELINE_OBJECTS(MissSubActions, SubActionInitData, MissSubAction);
	PIPELINE_OBJECTS(FailSubActions, SubActionInitData, FailSubAction);

	PIPELINE_FLOAT(OffenseInfluence, -1.0f);
	PIPELINE_FLOAT(SpecialOffenseInfluence, -1.0f);

	PIPELINE_FLOAT(MaxHPInfluence, 0.0f);
	PIPELINE_FLOAT(CurrentHPInfluence, 0.0f);
	PIPELINE_FLOAT(DefenseInfluence, 0.0f);
	PIPELINE_FLOAT(SpecialDefenseInfluence, 0.0f);
	PIPELINE_FLOAT(SpeedInfluence, 0.0f);

	PIPELINE_FLOAT(SelfTargetMod, 1.0f);
	PIPELINE_BOOL(AlwaysVisible, false);

	PIPELINE_BOOL(CopyStatusToMe, false);
	PIPELINE_BOOL(CopyStatusFromMe, false);
	PIPELINE_BOOL(MoveStatusToMe, false);
	PIPELINE_BOOL(MoveStatusFromMe, false);
	PIPELINE_BOOL(RandomizeStatus, false);

	bool m_bIgnoreStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action will be performed even if the owner has any of the status effects represented in this bitmask \n Example: IgnoreStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner has all of the status effects represented in this bitmask \n Example: RequireStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireAnyStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner has at least one of the status effects represented in this bitmask \n Example: RequireAnyStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireNotStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner has none of the status effects represented in this bitmask \n Example: RequireNotStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireMissingAnyStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner is missing at least one of the status effects represented in this bitmask \n Example: RequireMissingAnyStatusEffects="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */

	PIPELINE_FLOAT(InflictDelay, 0.0f); /**< This action will impose a delay on the target's next turn \n Example: InflictDelay=0.2f \n Default: 0.0f */
	PIPELINE_FLOAT(ThreatPen, 1.0f); /**< How much this action will disregard Threat in targeting, 0.0f = always choose highest Threat, 1.0f = normal, 100.0f = ignore Threat \n Example: ThreatPen=0.0f \n Default: 1 */
	PIPELINE_FLOAT(TargetDefenseModForDivision, 1.0f);

	int m_iStatusEffect[NUM_STATUS_EFFECTS_PER_ACTION]; /**< Adds status effect to this action \n Example: StatusEffect1="Poison" StatusEffect2="Confuse" StatusEffect3="Regen" \n Options: See StatusEffects */
	int m_iStatusEffectHeal[NUM_STATUS_EFFECTS_PER_ACTION]; /**< Heals status effect with this action \n Example: StatusEffectHeal1="Poison" StatusEffectHeal2="Confuse" StatusEffectHeal3="Regen" \n Options: See StatusEffects */
	PIPELINE_FLOAT(StatusEffectIntensity, -1.0f); /**< Intensity of status effect applied. What the intensity is used for is determined on a game by game basis. \n Example: StatusEffectIntensity="2" \n Default: 1 */
	PIPELINE_FLOAT(StatusEffectChance, 1.0f); /**< The chance that this status effect will be applied. \n Example: StatusEffectChance="0.5" \n Default: 1 */
	PIPELINE_FLOAT(StatusEffectPower, 0.0f); /**< The potency of the status effect applied \n Example: StatusEffectPower="1.5" \n Default: 1.0 */
	PIPELINE_INT(MaxNumStatusEffectsToHeal, NUM_STATUS_EFFECTS_PER_ACTION); /**< Heals no more than this number of status effects. Useful if you want to specify multiple status effects that can be healed, but you want less than all to be healed. \n Example: MaxNumStatusEffectsToHeal="1" \n Default: Infinite */
	PIPELINE_BOOL(PermanentStatusEffect, false); /**< Status effects are permanent on the target. \n PermanentStatusEffect="true" \n Default: False */

	PIPELINE_INT(MenuIndex, -1);
};

/**
*  Action to use in battle
*/
class Action : public GameObject
{
	REGISTER_CLASS_DECL(Action, "Action", GameObject)
public:
	friend class List<Action>;

	Action(ObjectInitData *pInitData);
	virtual ~Action();

	void ReInitialize();
	virtual void Destroy();
	//bool IsEqual(Action* pOther){ return (m_uActionId & pOther->GetHash())==m_uActionId; }
	//u32 GetHash(){ return m_uActionId; }
	virtual ActionTargetType GetActionTargetType() { return m_eActionTargetType; }
	void SetActionTargetType(ActionTargetType eType) { m_eActionTargetType = eType; }
	BaseEntity* GetOwner() { return m_pOwner; }
	virtual void SetOwner(BaseEntity* pOwner);
	virtual void SetTarget(int iTargetID, int iTargetGroupID) { m_iTargetID = iTargetID; m_iTargetGroupID = iTargetGroupID; }
	virtual void SetTarget(BaseEntityHandle* pTarget, bool bAssertOnTargetIDInvalid = true);
	virtual void SetTargets(BaseEntityHandle** pTargetList, int iListSize);
	virtual void SetTargetType(ActionTargetType eType) { m_eActionTargetType = eType; }
	virtual void SetExecutionPosition(POSITION_VECTOR vPos) { m_vExecutionPosition = vPos; }
	POSITION_VECTOR GetExecutionPosition() { return m_vExecutionPosition; }
	virtual bool HasExecutionPosition();
	virtual bool UsesExecutionPosition() { return m_bUsesExecutionPosition; }
	virtual void AddTarget(BaseEntityHandle* pTarget);
	virtual void RemoveTarget(BaseEntityHandle* pTarget);
	virtual void RemoveDeadTargets();
	virtual void RemoveDestroyedTargets();
	virtual void ResolveTargets();
	virtual void DerivedResolveTargets() {}
	void GetTargetInfo(int* iTargetID, int* iTargetGroupID) { *iTargetID = m_iTargetID; *iTargetGroupID = m_iTargetGroupID; }
	CharacterReference GetTargetReference() { return m_ActionTarget; }
	virtual BaseEntityHandle** GetTargetList() { return &(m_pTargetList[0]); }
	virtual BaseEntityHandle** GetPrevTargetList() { return &(m_pPrevTargetList[0]); }
	virtual int GetIndexOfTarget(BaseEntityHandle* pTarget);
	virtual BaseEntityHandle* GetRandomValidTarget();
	int GetNumTargets() { return m_iNumTargets; }
	virtual bool HasValidTargets();
	virtual bool IsTargetValid(BaseEntityHandle* pTarget);
	bool* GetIgnoreStatusEffectBitmask() { return m_bIgnoreStatusEffects; }
	virtual int GetActionType() { return ACTIONTYPE_DEFAULT; }
	virtual void Execute(BaseEntity* pOwner, BaseEntityHandle* pTarget = NULL);
	virtual void PostExecute(BaseEntityHandle* pTarget);
	virtual void PrepareForExecution();
	virtual bool CanMiss();
	virtual bool CanTargetNext();
	virtual bool AreStatusEffectRequirementsSatisfied(BaseEntity* pOwner);
	virtual void ExecuteGlobalMissAction();
	virtual void ExecuteHitActionOnTarget(BaseEntity* pOwner, BaseEntityHandle* pTarget = NULL);
	virtual void ExecuteMissAction(BaseEntityHandle* pTarget);
	virtual void ExecuteFailAction(BaseEntityHandle* pTarget);
	bool PassesConditions(BaseEntityHandle* pTarget);
	virtual bool IsCompareable(Action* pCheck);
	virtual bool IsEquivalent(Action* pCheck);
	const wchar_t* GetDescriptionText();
	int GetDescriptionLoc() { return m_iDescriptionTextLocID; }
	static u32 GetNextActionId() { return ++g_uActionId; }
	DamageType GetDamageType() { return m_eDamageType; }
	ElementDamageType GetElementDamageType() { return m_eElementDamageType; }
	ActionDisplayType GetActionDisplayType() { return m_eActionDisplayType; }

	bool IsTargetingSelf();
	bool ShouldExecuteIfOwnerIsDead() { return m_bExecuteIfOwnerDead; }
	void SetExecuteIfOwnerIsDead(bool bSet) { m_bExecuteIfOwnerDead = bSet; }
	void SetSubActionOwner(Action* pAction) { m_pSubActionOwner = pAction; }
	Action* GetSubActionOwner() { return m_pSubActionOwner; }

	float GetPrepTime() { return m_fPrepTime; }
	float GetTurnCost() { return m_fTurnCost; }
	int GetActionWeight() { return m_iWeight; }
	void NextTurn();

	/** Check if this action doesn't need a target */
	virtual bool CheckImmediatelyValid(BaseEntityHandle* pOwner);
	virtual bool RequiresPositioning();

	virtual void UpdatePreEffects(float fDeltaTime);
	virtual void Update(float fDeltaTime);
	virtual void UpdatePostEffects(float fDeltaTime);
	virtual bool IsDonePreEffects();
	virtual bool IsDoneExecuting();
	virtual bool IsDonePostEffects();
	virtual	ScriptedScene* PlayPreEffects();
	virtual ScriptedScene* PlayEffects();
	virtual ScriptedScene* PlayPostEffects();
	virtual void ForceEndEffects();
	ActionEffectSet* GetActionEffectSet() { return &m_ActionEffectSet; }
	virtual void ApplyPreAnimationModifiers();
	virtual void ApplyPostAnimationModifiers();
	virtual int GetNumActions(bool& bSequential) { return 1; }
	virtual void OnFinished();
	virtual bool RequiresTarget() {
		return true;
	}

	//Used for games like CoE in which you can back track through the action selection of characters, allowing an action to do something upon being unselected.
	//For example, the UseItemAction informs the inventory manager that the item in question is no longer queued up to be used.
	virtual void DiscardAction() {}

	//Can override in derived classes to do a check to see if the action will actually provide a meaningful effect
	virtual bool WillHaveAnEffect() { return true; }
	virtual bool WillHaveAnEffectOnTarget(BaseEntityHandle* pTarget) { return true; }
	virtual bool WillHaveAnEffectOnTarget(Engine::SaveData* pSaveData, int iHeroIndex) { return true; }

	virtual int GetLevelRequirement();
	virtual bool CanBeExecutedAtLevel(int iLevel) { return true; }
	virtual int GetJobLevelRequirement() { return m_iJobLevelRequirement; }
	virtual bool CanBeExecutedAtJobLevel(int iLevel) { return true; }
	float GetCooldown() const { return m_fCooldown; }
	float GetCurrentCooldown() const { return m_fActualCooldown; }
	bool CanUseOnOverworld() { return m_bOverworldUse; }
	virtual int GetUseLocID() { return m_iUseTextLocID; }
	virtual const wchar_t* GetUseText();
	virtual const wchar_t* GetUseSelfText();
	virtual const wchar_t* GetTargetText();
	const wchar_t* GetFailText();
	bool HasFinishText() { return m_iFinishTextLocID>0; }
	const wchar_t* GetFinishText();
	bool FailSilently() { return m_bFailSilently; }
	bool NoShowOwnerInUseText() { return m_bNoShowOwnerInUseText; }
	virtual int GetMPCost();
	virtual void SetMPCost(int iSet) {
		m_iMPCost = iSet;
	}
	virtual int GetHPCost();
	StringHash GetGotoActionList() { return m_sGotoActionList; }
	void SetAbilityHash(StringHash sHash) { m_uAbilityHash = sHash; }
	StringHash GetAbilityHash() { return m_uAbilityHash; }

	void CalculateExecutionSpeed();
	float GetSpeed() { return m_fSpeed; }
	virtual float GetSpeedModifier() { return m_fSpeedModifier; }
	void SetSpeedModifier(float fMod) { m_fSpeedModifier = fMod; }
	bool IsSlowerThan(Action* pAction);
	virtual bool CanTargetDead() { return m_bCanTargetDead; }

	bool CanExecuteSubActions() { return m_bCanExecuteSubActions; }
	void SetCanExecuteSubActions(bool bCan) { m_bCanExecuteSubActions = bCan; }

	float GetTargetDamageMultiplier(BaseEntityHandle* pTarget);

	int PlayHitSound();
	int PlayMissSound();
	int PlayCriticalSound();
	virtual StringHash GetAnim();
	void SetAnimation(StringHash sAnim) { m_sAnim = sAnim; }
	void SetAnimation(char* sAnim) { m_sAnim = ToStringHash(sAnim); }

	virtual bool HasActionFailed() { return m_bFailed; }
	virtual bool DidActionMiss() { return m_bMissed; }
	void SetFailed() { m_bFailed = true; }
	bool IsLocked() { return m_bLocked; }
	void SetLock(bool bLocked) { m_bLocked = bLocked; }
	bool EnsureValidTarget();
	virtual bool IsSilent() { return m_bExecuteSilently; }
	void DoTargetResolution(bool bDo) { m_bDoTargetResolution = bDo; }
	void SetCanCancel(bool bCanCancel) { m_bCanBeCanceled = bCanCancel; }
	void SetCanChangeTarget(bool bCanChange) { m_bCanChangeTarget = bCanChange; }
	bool CanBeCanceled() { return m_bCanBeCanceled; }
	void SetIsPurgedFromQueueWhenOwnerDies(bool bPurge) { m_bPurgeFromQueueWhenOwnerDies = bPurge; }
	bool IsPurgedFromQueueWhenOwnerDies() { return m_bPurgeFromQueueWhenOwnerDies; }

	const wchar_t* GetLocalizedName();

	//For random selection
	inline void IncrementNumAttempts() { m_iNumAttempts++; }
	inline void IncrementNumChosen() { m_iNumChosen++; }
	bool RequiresExecution();
	bool CanPassRandomization();

	List<ActionComboOwner>* GetComboOwners() { return &m_aActionComboOwners; }

	virtual bool IsHealAction() { return false; }

	void ForceToIgnoreAilments();

	ScriptedScene* GetScriptedScene() { return m_pScriptedScene; }

	inline float GetAccuracy() { return m_fAccuracyModifier; }
	float GetThreatPen();
	inline int GetMenuIndex() { return m_iMenuIndex; }
	inline float GetInflictDelay() { return m_fInflictDelay; }
	inline bool IsAlwaysVisible() { return m_bAlwaysVisible; }

	List<SubAction>* GetSubActions() { return &m_pSubActions; }

public:
	//u32 m_uActionId; Use in the case that we reduce actions to a u32 to do integer comparisons
	u32 m_uAbilityHash;
	ActionTargetType m_eActionTargetType; /**< Type of target for the action \n Example: ActionTargetType="Single Enemy" \n Default: Me \n Options: Me, Single Enemy, Single Ally, Other Ally, Random Ally, Area, Enemy Group, Random Enemy, Random Target, All Enemies, All Allies, All, Last Attacker, Any */
	ActionType m_eActionType; /**< Type of action \n Example: ActionType="Melee" \n Default: Melee \n Options: Melee, Ranged, Magic, Buff, Protection, Reaction, NA, Any */
	CharacterReference m_ActionOwner;
	ActionTargetType m_eActionOwnerType; /**< Type of target for the action \n Example: ActionTargetType="Single Enemy" \n Default: Me \n Options: Me, Single Enemy, Single Ally, Other Ally, Random Ally, Area, Enemy Group, Random Enemy, Random Target, All Enemies, All Allies, All, Last Attacker, Any */
	CharacterReference m_ActionTarget;
	ActionTargetState m_eActionTargetState;
	int m_iTargetID;
	int m_iTargetGroupID;
	BaseEntityHandle* m_pTargetList[MAX_NUM_TARGETS];
	int m_iNumTargets;
	BaseEntityHandle* m_pPrevTargetList[MAX_NUM_TARGETS];
	int m_iPrevNumTargets;
	BaseEntity* m_pOwner;
	u32 m_uActionId;
	static u32 g_uActionId;
	ActionEffectSet m_ActionEffectSet;

	float m_fPrepTime; /**< How long does this action take to be prepared after it is selected \n Example: ActionTime="1.0" \n Default: 2.0 */
	float m_fTurnCost; /**< For systems where an action may cost more or less than 1 full turn \n Example: TurnCost="0.5" \n Default: 1.0 */
	float m_fCooldown; /**< Cooldown after this action is used \n Example: Cooldown="1.25" \n Default: 1.0 */

	float m_fActualCooldown;

	float m_fSpeedModifier; /**< Game-dependent speed value that will modify action speed value to influence turn order (higher valued actions go before lower valued) \n Example: SpeedModifier="2" \n Default: 1.0 */
	float m_fAccuracyModifier; /**< Multiply owner's accuracy value by this for accuracy calculation \n Example: AccuracyModifier="2.0" \n Default: 0.95 */

							   //For random selection
	int m_iNumChosen;
	int m_iNumAttempts;

	float m_fSpeed;

	POSITION_VECTOR m_vExecutionPosition;

	int m_iWeight; /**< Weight value for the AI's random selection \n Example: Weight="200" \n Default: 100 */
	int m_iWeightIncreasePerTurn; /**< Weight value increase per turn for the AI's random selection \n Example: WeightIncreasePerTurn="10" \n Default: 0 */
	int m_iWeightChangeAfterUse; /**< Weight value change after use for the AI's random selection \n Example: WeightChangeAfterUse="10" \n Default: 0 */
	int m_iMinWeight; /**< Min weight this action can ever have after after modifications \n Example: MinWeight="10" \n Default: 0 */
	int m_iMaxWeight; /**< Max weight this action can ever have after after modifications \n Example: MaxWeight="1000" \n Default: 100000000 */
	float m_fRandomFactor; /**< Modify final numeric value (damage, healing, etc.) by +/- this value as a percentage \n Example: RandomFactor="0.1" \n Default: 0.25 */
	StringHash m_sGotoActionList; /**< Name of Action List for the AI to select after this action is used \n Example: GotoActionList="MyOtherList" \n Default: None */
	StringHash m_sTargetGotoActionList; /**< Name of Action List for the target to select after this action is used \n Example: TargetGotoActionList="MyOtherList" \n Default: None */
	StringHash m_sMessage; /**< Name of the message to send to the target \n Example: Message="NotifySomething" \n Default: None */
	StringHash m_sAnim; /**< Animation to play for this action.\nExample: Anim="Dodge" \n Default: Attack */
	StringHash m_sFinishAnim; /**< Animation to play for this action when it finishes.\nExample: Anim="Dodge" \n Default: None */

	int m_iUseTextLocID; /**< Localization string ID to display upon the use of this action \n Example: UseText="LOC_ATTACK" \n Default: None */
	int m_iTargetTextLocID; /**< Localization string ID to display for the target of this action \n Example: TargetText="LOC_POISON" \n Default: None */
	int m_iUseSelfTextLocID; /**< Localization string ID to display when the target of this action is the owner \n Example: TargetText="LOC_POISON" \n Default: None */
	int m_iDescriptionTextLocID; /**< Localization string ID to display for the description of this action \n Example: DescriptionText="LOC_ATTACK_DESCRIPTION" \n Default: None */
	int m_iMissTextLocID; /**< Localization string ID to display for when this action misses \n Example: MissText="LOC_FAILURE" \n Default: LOC_MISS_MESSAGE */
	int m_iFailTextLocID; /**< Localization string ID to display for when this action fails to execute successfully \n Example: FailText="LOC_FAILURE" \n Default: LOC_ACTION_FAIL */
	int m_iFinishTextLocID; /**< Localization string ID to display when this action finishes \n Example: FinishText="LOC_FINISH" \n Default: None */
	int m_iLevelRequirement; /**< Level requirement to use this action \n Example: Level="5" \n Default: 1 */
	int m_iCitizen;
	int m_iJobLevelRequirement; /**< Job level requirement to use this action \n Example: Level="5" \n Default: 1 */
	int m_iMPCost; /**< MP cost of this action \n Example: MPCost="5" \n Default: 0 */
	float m_fHPCostPercent; /**< Percentage HP cost of this action \n Example: HPCostPercent="0.1" \n Default: 0 */
	DamageType m_eDamageType; /**< Type of damage. Informs what stat to use in the damage equation \n Example: DamageType="Melee" \n Default: Melee \n Options: Melee, Magic*/
	ElementDamageType m_eElementDamageType; /**< Type of elemental damage that informs what kind of elemental defense is applied against this action \n Example: ElementDamageType="Fire" \n Default: None \n Options: None, Fire, Ice, Lightning */
	PLAT_BOOL m_bExecuteSilently; /**< Flag indicating if this action should execute without displaying text \n Example: Silent="True" \n Default: False */
	PLAT_BOOL m_bDoTargetResolution;
	PLAT_BOOL m_bOverworldUse; /**< Flag indicating whether this action can be used on the overworld (i.e. outside of battle) \n Example: Overworld="True" \n Default: False */
	PLAT_BOOL m_bApplyAccuracyGlobally; /**< Flag indicating whether the accuracy test is done once globally for all targets, rather than once per target \n Example: GlobalAccuracy="False" \n Default: False */
	PLAT_BOOL m_bCanMiss; /**< Flag indicating whether this action can miss \n Example: CanMiss="False" \n Default: True */
	PLAT_BOOL m_bCanTargetDead; /**< Flag indicating whether this action can target the dead \n Example: CanTargetDead="True" \n Default: False */
	PLAT_BOOL m_bCanTargetNext; /**< Flag indicating whether this action can target the next target if the proper target is dead \n Example: CanTargetNext="False" \n Default: True */
	PLAT_BOOL m_bFailSilently; /**< Flag indicating whether this action should fail without printing text \n Example: FailSilently="True" \n Default: False */
	PLAT_BOOL m_bNoShowOwnerInUseText; /**< Flag indicating whether the owner should be ignored in the use text \n Example: NoShowOwnerInUseText="True" \n Default: False */
	PLAT_BOOL m_bEnsureValidTarget; /**< Flag indicating that the AI will ensure the action has a valid target before selecting it \n Example: EnsureValidTarget="True" \n Default: False */
	PLAT_BOOL m_bOnlyChooseIfWillHaveEffect; /**< Flag indicating that the AI will only choose this action if it will actually have an effect \n Example: OnlyChooseIfWillHaveEffect="True" \n Default: False */
	PLAT_BOOL m_bExecuteOnlyIfOwnerDead; /**< Flag indicating that action will only execute even if the owner is dead and not otherwise \n Example: ExecuteOnlyIfOwnerDead="True" \n Default: False */
	PLAT_BOOL m_bExecuteIfOwnerDead; /**< Flag indicating that action will still execute even if the owner is dead \n Example: ExecuteIfOwnerDead="True" \n Default: False */
	PLAT_BOOL m_bCanExecuteSubActions;
	PLAT_BOOL m_bCanBeCanceled; //Can be canceled by the CancelAction property of SupportAction
	PLAT_BOOL m_bLocked; //This action has been locked for some reason, and is not available for selection
	PLAT_BOOL m_bFailed; //Set if the action fails for some reason before executing (possibly it had no targets to resolve)
	PLAT_BOOL m_bMissed; //Set if the action misses
	PLAT_BOOL m_bCanChangeTarget;
	PLAT_BOOL m_bPurgeFromQueueWhenOwnerDies; //This action will be purged from the BattleManager when the owner dies
	PLAT_BOOL m_bUsesExecutionPosition;
	PLAT_BOOL m_bCanBeCountered;
	
	float m_fDrawAggroChance; /**< Chance to draw aggro from target \n Example: AggroChance="0.5" \n Default: 0.0 */
	float m_fAggroPersistChance; /**< Chance for aggro from this action to persist from turn to turn \n Example: AggroPersistChance="0.75" \n Default: 1.0 */

	float m_fTargetHealthMultiplier; /**< Multiply by target health to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of target health. \n Example: TargetHealthMultiplier="2" \n Default: 0 */
	float m_fUserHealthMultiplier; /**< Multiply by user health to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of target health. \n Example: UserHealthMultiplier="2" \n Default: 0 */
	float m_fUserMPMultiplier; /**< Multiply by user health to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of target health. \n Example: UserHealthMultiplier="2" \n Default: 0 */
	float m_fExemptFromMultipliers; /**< Portion of action effectiveness to not receive any multipliers \n Example: ExemptFromMultipliers="0.5" \n Default: 0.0 */

	float m_fTargetTurnDamageMultiplier; /**< Multiply by damage received this turn to generate action effectiveness (as determined by derived actions). If a negative value, multiply by inverse of damage. \n Example: TargetDamageReceivedMultiplier="2" \n Default: 0 */
	float m_fConsecutiveUseMultiplier; /**< Multiply by the number of consecutive uses of this action to generate action effectiveness (as determined by derived actions). \n Example: ConsecutiveUseMultiplier="2" \n Default: 1.0 */
	int m_iMaxConsecutiveUseBoost; /**< The maximum number of consecutive uses to use with the ConsecutiveUseMultiplier value. \n Example: MaxConsecutiveUseBoost="4" \n Default: 1 */

	ActionDisplayType m_eActionDisplayType;/**< Type of display for this action. \n Example: ActionDisplayType="Binary" \n Default: Normal \n Options: Normal, Hex, Binary */

	float m_fOffenseInfluence;
	float m_fSpecialOffenseInfluence;

	float m_fMaxHPInfluence;
	float m_fCurrentHPInfluence;
	float m_fDefenseInfluence;
	float m_fSpecialDefenseInfluence;
	float m_fSpeedInfluence;

	float m_fSelfTargetMod;
	bool m_bAlwaysVisible;

	bool m_bCopyStatusToMe;
	bool m_bCopyStatusFromMe;
	bool m_bMoveStatusToMe;
	bool m_bMoveStatusFromMe;
	bool m_bRandomizeStatus;

	bool m_bIgnoreStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action will be performed even if the owner has any of the status effects represented in this bitmask \n Example: IgnoreStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner has all of the status effects represented in this bitmask \n Example: RequireStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireAnyStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner has at least one of the status effects represented in this bitmask \n Example: RequireAnyStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireNotStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner has none of the status effects represented in this bitmask \n Example: RequireNotStatusEffect="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */
	bool m_bRequireMissingAnyStatusEffects[MAX_NUM_STATUS_EFFECTS]; /**< This action can only be performed if the owner is missing at least one of the status effects represented in this bitmask \n Example: RequireMissingAnyStatusEffects="STATUS_EFFECT_CONFUSE STATUS_EFFECT_SLEEP" \n Default: None */

	float m_fInflictDelay;
	float m_fTargetDefenseModForDivision; /**< Used to penetrate defense in a game with a \n division formula \n Example: TargetDefenseModForDivision="0.5" */

	int m_iStatusEffect[NUM_STATUS_EFFECTS_PER_ACTION]; /**< Adds status effect to this action \n Example: StatusEffect1="Poison" StatusEffect2="Confuse" StatusEffect3="Regen" \n Options: See StatusEffects */
	int m_iStatusEffectHeal[NUM_STATUS_EFFECTS_PER_ACTION]; /**< Heals status effect with this action \n Example: StatusEffectHeal1="Poison" StatusEffectHeal2="Confuse" StatusEffectHeal3="Regen" \n Options: See StatusEffects */
	float m_fStatusEffectIntensity; /**< Intensity of status effect applied. What the intensity is used for is determined on a game by game basis. \n Example: StatusEffectIntensity="2" \n Default: 1 */
	float m_fStatusEffectChance; /**< The chance that this status effect will be applied. \n Example: StatusEffectChance="0.5" \n Default: 1 */
	float m_fStatusEffectPower; /**< The potency of the status effect applied. \n Example: StatusEffectPower="1.5" \n Default: 1 */
	PLAT_BOOL m_bPermanentStatusEffects; /**< Status effects are permanent on the target. \n PermanentStatusEffect="true" \n Default: False */
	int m_iMaxNumStatusEffectsToHeal; /**< Heals no more than this number of status effects. Useful if you want to specify multiple status effects that can be healed, but you want less than all to be healed. \n Example: MaxNumStatusEffectsToHeal="1" \n Default: Infinite */

	int m_iMenuIndex;

protected:
	Action* m_pSubActionOwner;

	List<SubAction> m_pSubActions;
	List<SubAction> m_pMissSubActions;
	List<SubAction> m_pFailSubActions;
	ListContainer<ActionCondition*> m_aActionConditionList;
	List<ActionComboOwner> m_aActionComboOwners;

	ScriptedScene* m_pScriptedScene;

private:
	Action* next;
	float m_fBaseThreatPen;
};

enum ActionListTrigger
{
	eActionListTrigger_Health = 0,
	eActionListTrigger_NumRounds,
	eActionListTrigger_AllyDeath,
	eActionListTrigger_Damaged,
	eActionListTrigger_HashName,
	NUM_ACTION_LIST_TRIGGERS
};

#ifdef PIPELINE_TOOL
static const char* g_szActionListTriggers[NUM_ACTION_LIST_TRIGGERS] =
{
	"Health",
	"NumRounds",
	"AllyDeath",
	"Damaged",
	"HashName"
};
#endif

struct ActionListInitData : public ObjectInitData
{
	ActionListInitData();
	virtual ~ActionListInitData();

#ifdef PIPELINE_TOOL
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual ObjectInitData* FindObject(const char* sName);
	virtual ObjectInitData* FindAction(const char* sName);
#else
	virtual void ReadFromFile(FileCollection* saveFile);
#endif

	PIPELINE_ENUM(Trigger, g_szActionListTriggers, NUM_ACTION_LIST_TRIGGERS, NUM_ACTION_LIST_TRIGGERS);
	PIPELINE_INT(TriggerParam, -1);
	PIPELINE_STRINGHASH(TriggerHashName, INVALID_STRINGHASH);

	PIPELINE_LOCID(ListName, 0);
	PIPELINE_LOCID(ListDesc, 0);
	PIPELINE_INT(Duration, -1);
	PIPELINE_INT(NumTimesChooseable, -1);
	PIPELINE_STRINGHASH(NextListOnDurationEnd, 0);
	PIPELINE_ENUM(FormationRequirement, g_szFormationTypes, NUM_FORMATIONS, NUM_FORMATIONS);

	// requirements for using Action Lists in AI scripts
	PIPELINE_BOOL(IgnoreList, false);
	PIPELINE_INT(Turn, -1);
	PIPELINE_INT(TurnBefore, -1);
	PIPELINE_INT(TurnAfter, -1);
	PIPELINE_INT(HealthBelow, -1);
	PIPELINE_INT(HealthAbove, -1);
	PIPELINE_INT(NumEnemiesBelow, -1);
	PIPELINE_INT(NumEnemiesAbove, -1);
	PIPELINE_INT(AnyAllyHealthBelow, -1);
	PIPELINE_INT(AnyAllyHealthAbove, -1);
	PIPELINE_INT(TurnMultiple, -1); // Only use on turns that are a multiple of x
	PIPELINE_INT(TurnOffset, -1); // Used with TurnMultipleOf to specify remainder

	bool m_bRequireStatusEffects[MAX_NUM_STATUS_EFFECTS];
	bool m_bRequireAnyStatusEffects[MAX_NUM_STATUS_EFFECTS];
	bool m_bRequireNotStatusEffects[MAX_NUM_STATUS_EFFECTS];
	bool m_bRequireMissingAnyStatusEffects[MAX_NUM_STATUS_EFFECTS];

	ActionListInitData* pNext;
	ActionInitData *pActions;		// linked list of actions
};

class ActionList : public Object
{
	REGISTER_CLASS_DECL(ActionList, "ActionList", Object)
public:
	friend class List<ActionList>;

	/** Constructors */
	ActionList(ObjectInitData *pInitData);

	/** Destructors */
	virtual ~ActionList();

	/** Functions **/
	const wchar_t* GetListName();
	const wchar_t* GetListDesc();
	int GetListLocID() { return m_iNameLocID; }
	bool HasOverworldSkills() { return m_bHasOverworldSkills; }
	bool HasSkillsAtLevel(BaseEntity* pEntity, Action** pAction = 0, bool bExact = false);
	int GetNextSkillLevel(int iCurrLevel);
	List<Action>& GetActions() { return m_aActionList; }
	Formation GetFormation() { return m_eFormationRequirement; }
	bool IsLocked() { return m_bLocked; }
	void SetLock(bool bLocked) { m_bLocked = bLocked; }
	bool CanChoose() { return m_iNumTimesChooseable != 0; }
	void Choose() { m_iNumTimesChooseable--; }
	StringHash GetNextListName() { return m_sNextListOnDurationEnd; }
	ActionListTrigger GetTrigger() { return m_eTrigger; }
	bool HasAction(Action* pAction);

	/** Variables **/
	ActionListTrigger m_eTrigger;
	int m_iTriggerParam;
	int m_iTriggerHash;
	int m_iNameLocID;
	int m_iDescLocID;
	int m_iDuration;
	StringHash m_sNextListOnDurationEnd;
	List<Action> m_aActionList;
	Action* GetActionByIndex(int iIndex);
	Action* GetUsableActionByIndex(int iIndex);
	int m_iTriggerTurn;
	//StringHash m_hTriggerHasStatusEffect;

	bool m_bIgnoreList;
	int m_iTurn;
	int m_iTurnBefore;
	int m_iTurnAfter;
	int m_iHealthBelow;
	int m_iHealthAbove;
	int m_iNumEnemiesBelow;
	int m_iNumEnemiesAbove;
	int m_iAnyAllyHealthBelow;
	int m_iAnyAllyHealthAbove;
	int m_iTurnMultiple;
	int m_iTurnOffset;
	int m_iCurrentDuration;

	bool m_bRequireStatusEffects[MAX_NUM_STATUS_EFFECTS];
	bool m_bRequireAnyStatusEffects[MAX_NUM_STATUS_EFFECTS];
	bool m_bRequireNotStatusEffects[MAX_NUM_STATUS_EFFECTS];
	bool m_bRequireMissingAnyStatusEffects[MAX_NUM_STATUS_EFFECTS];


private:
	ActionList* next;

	PLAT_BOOL m_bHasOverworldSkills;
	PLAT_BOOL m_bLocked; //This action list has been locked for some reason, and is not available for selection
	Formation m_eFormationRequirement;
	int m_iNumTimesChooseable;
};