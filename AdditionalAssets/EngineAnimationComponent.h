#ifndef __EngineAnimationComponent_H__
#define __EngineAnimationComponent_H__
#pragma once

#include "EngineAnimationInterfaceComponent.h"
#include "EngineIRenderable.h"

// Forward Declarations
// --------------------
namespace Engine
{
	class Entity;
	class SpriteAnimationSet;
	class SpriteAnimation;
	class TileAnimation;
	class TileAnimationData;
	class AnimationBatch;
	class Surface;
	struct RenderBatch;
	struct AnimInfo;
	struct AnimationComponentInitData;
}

namespace Engine
{
	struct AnimationComponentInitData : public AnimationInterfaceComponentInitData
	{
		AnimationComponentInitData();
#ifdef PIPELINE_TOOL
		virtual bool InitializeSaveData(XMLElement* pElement);
		virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
		virtual bool ParseNecessaryEditorProperties(XMLElement* pElement, bool bIgnoreSource);
		virtual void ApplyMirrorValue(MirrorState eMirror);

		virtual bool RetrieveGID() { return true; }

		virtual void VerifyAnimFilename(bool& bRetVal);

		static PLAT_BOOL m_bRandomColours;
		static PLAT_BOOL m_bRandomAnimation;
		static float m_fLayerOpacity;

		bool m_bUsingTilesetAnim;
		int m_iRandomAnimFileIndex;

		virtual void WriteToFile(std::fstream* saveFile);

#else
		virtual void ReadFromFile(FileCollection* pSaveFile);
#endif

#if USING_DYNAMIC_STRINGS
		PIPELINE_BASICSTRING(AnimFileName, "");
#else
		PIPELINE_BASICSTRING_SIZE(AnimFileName, "", 256);
#endif

		virtual const char* GetDebugString() { return AnimFileName.sString.ToChar(); }

		PIPELINE_FLOAT(Rotation, 0.0f);
		PIPELINE_RANDOM(RandomizedRotation, 0.0f, 0.0f);

		PIPELINE_ENUM(HorizontalAlignment, g_szAlignmentStrings, NUM_ALIGNMENTS, eAlignment_Centre);
		PIPELINE_ENUM(VerticalAlignment, g_szAlignmentStrings, NUM_ALIGNMENTS, eAlignment_Centre);
		PIPELINE_ENUM(Mirror, g_szMirrorStrings, NUM_MIRROR_STATES, eMirror_None);
		PIPELINE_RANDOM(RandomizedMirror, 0, 0);

		PIPELINE_BOOL(UseCollisionSize, false);
		PIPELINE_BOOL(RandomizeInitialFrame, false);
		PIPELINE_BOOL(RandomizeVertexColours, false);
		PIPELINE_BOOL(ScaleTexCoordsWithScale, false);
		PIPELINE_BOOL(RotateBeforeScale, false);

		//Can't (or at least don't need...) to change these to PIPELINE_VECTOR2 since they're populated by the builder, not manually
		Vector2 m_vTileScale;
		Vector2 m_vUpperLeftTexCoords;
		Vector2 m_vLowerRightTexCoords;

		PIPELINE_COLOUR(Colour, 1.0f, 1.0f, 1.0f, 1.0f);
		PIPELINE_RANDOM(RandomizedDarken, 0.0f, 0.0f);
		PIPELINE_RANDOM(RandomizedOpacityChange, 0.0f, 0.0f);

		PIPELINE_VECTOR2(Scale, 1.0f, 1.0f);
		PIPELINE_RANDOM(RandomizedScale, 1.0f, 1.0f);

		PIPELINE_VECTOR2(RotationPointOffset, 0.0f, 0.0f);
	};

	class AnimationComponent : public AnimationInterfaceComponent, public IRenderInstruction
	{
		REGISTER_CLASS_DECL(AnimationComponent, "AnimationComponent", AnimationInterfaceComponent)
	public:
		friend class List<AnimationComponent>;

		/** Constructor
		*	@param animFileName The relative path to the source texture file.
		*/
		AnimationComponent(ObjectInitData* pInitData);
		virtual ~AnimationComponent();

		virtual int Initialize();
		virtual int PostInitialize();
		virtual void Destroy();
		void UnloadAnimations(bool bDeleteAnimations = false);

		virtual void DerivedUpdate(float fDeltaTime);

		//void static StaticUpdate(float fDeltaTime);

		//Activate or deactivate this component. Informs the batch animation system
		virtual void Activate();
		virtual void Deactivate();

		virtual void ExecuteInstruction() {}

		/** Plays a single animation on the sprite.
		@param animName The name of the animation to play
		@param bResetTime True to start the animation at the beginning
		@retrun bool The animation was found and started playing successfully
		*/
		virtual bool DerivedPlayAnimation(StringHash sName, bool bResetTime, bool bDoNotifications);
		void StartNewSequence(bool bDoNotifications);

		/** Plays a random animation from the animation set.
		*/
		virtual void PlayRandomAnimation(bool bCanPlayCurrent = true);

		/** Plays the animation from the animation set at index.
		*/
		virtual void PlayAnimationByIndex(int iIndex);

		/** Plays the next animation in the animation set. This is used only for the Anim Viewer utility, and isn't used in-game
		*/
		virtual void PlayNextAnimation();

		/** Plays the previous animation in the animation set. This is used only for the Anim Viewer utility, and isn't used in-game
		*/
		virtual void PlayPreviousAnimation();

		//void AddAnimation(const char* sAnimPath, const char* sAnimName, const char* sExtension=TEXTURE_EXT);

		virtual bool HasAnimation(StringHash sName);

#ifdef ON_THE_FLY_ANIMATION_LOADING
		StringHash GetInitialAnimNameHash() const { return m_sInitialAnimName; }
		void SetInitialAnimName(StringHash hName) { m_sInitialAnimName = hName; }
#endif

		/** Set the world space position of the actor. */
		virtual void SetWorldPosition(const POSITION_VECTOR& position, bool bDoAssetAndZoomScale = true);
		POSITION_VECTOR GetWorldPosition() { return m_vRenderPosition; }
		POSITION_VECTOR GetUnmodifiedWorldPosition();
		POSITION_VECTOR GetWorldCentrePosition();
		POSITION_VECTOR GetPosition(Alignment eHorizontalAlignment, Alignment eVerticalAlignment);

		inline Vector3 GetScale() const { return m_vScaleFactor; }

		void ScaleSize(float fScaleAmount);
		void ScaleSize(Vector2 vScale);
		void ScaleSize(Vector3 vScale);
		void ScaleX(float fScaleAmount);
		void ScaleY(float fScaleAmount);
		void ScaleXTexCoordsWithScale();
		void ScaleYTexCoordsWithScale();
		void ScaleTexCoordsWithScale();
		void DoScaleTexCoordsWithScale(bool bDo) { m_bScaleTexCoordsWithScale = bDo; }

		int GetNumAnimations();
		virtual int GetAnimationIndex();

		const char* GetAnimationNameByIndex(int iIndex);

		void SetLoop(bool loop);
		bool IsLooping();
		bool IsInSequenceLoop();
		StringHash GetSequenceName() const;
		void ResetTime();

		/** Returns the width, in pixels, of this Sprite. */
		virtual float GetWidth() const;

		/** Returns the height, in pixels, of this Sprite. */
		virtual float GetHeight() const;

		/** Returns the width, in pixels, of this Sprite with scale applied. */
		virtual float GetScaledWidth() const;

		/** Returns the height, in pixels, of this Sprite with scale applied. */
		virtual float GetScaledHeight() const;

		bool LoadNewAnimation(const char* sAnimPath);
		bool LoadNewAnimation(TileAnimationData* pData);

		/** Returns the length of the animation. */
		float GetAnimationLength(StringHash sName) const;
		float GetAnimationLength() const;

		bool IsDoneAnimation();
		bool IsDoneAnimationOrLooping();

		virtual void SetColour(float r, float g, float b);
		virtual void SetColour(float r, float g, float b, float a);
		virtual void SetColour(Vector3 vec);
		virtual void SetColour(Vector4 vec);
		virtual void SetOpacity(float alpha);
		Vector4 GetColour() { return m_vCustomColour; }
		float GetOpacity() { return m_vCustomColour.w; }

		/*unsigned int* GetTextureIDPointer();*/
		bool IsLoaded();

		void SetFrameRateTimeScale(float fFrameRateTimeScale);

		virtual void CalculateCullBounds(bool bActive, Vector2& vUpperLeftPos, Vector2& vLowerRightPos);

#ifdef TRACK_MEMORY
		const char* GetTextureName();

#ifdef PLAT_PC
		void PrintAnimState(Vector2 vPrintLocation);
#endif
#endif

#if (defined(PLAT_PC) || defined(PLAT_OSX)) && !defined(DIST) && !defined(PIPELINE_TOOL)
		void RegisterFunctions();
		void AnimInfoReloaded();
#endif

		AnimResource* GetAnimResource();

		//void SetBatch(AnimationBatch* pBatch){ m_pBatch = pBatch; }
		//AnimationBatch* GetBatch(){ return m_pBatch; }
		bool GetCachedLoaded() { return m_bLoaded; }
		virtual RenderBatch* PrepareRenderBatch();
		virtual void FinishRenderBatch();
		//PrimType GetPrimitiveType();
		void SetInitialTextureCoordinates(Vector2 upperLeft, Vector2 lowerRight);
		void SetInitialTextureCoordinates(Vector2 upperLeft, Vector2 upperRight, Vector2 lowerLeft, Vector2 lowerRight);	// Would like to do addresses, but it can cause an error on vita if constructing the object as the parameter
		void SetInitialTextureCoordinates(float fUpperLeftX, float fUpperLeftY, float fLowerRightX, float fLowerRightY);
		virtual void IncrementTextureCoordinates(float fXIncrement, float fYIncrement);
		virtual void IncrementTextureCoordinates(Vector2 vIncrement) { IncrementTextureCoordinates(vIncrement[0], vIncrement[1]); }
		virtual void ChangeTextureCoordinates(float fUpperLeftX, float fUpperLeftY, float fLowerRightX, float fLowerRightY);
		virtual void ChangeTextureCoordinates(Vector2 upperLeft, Vector2 lowerRight);
		virtual void ChangeTextureCoordinates(Vector2 upperLeft, Vector2 upperRight, Vector2 lowerLeft, Vector2 lowerRight);

		void SetTileAnimationData(TileAnimationData* pData) { m_pTileAnimationData = pData; }

		/** Changes whether or not the sprite is rendered at its centre. */
		void SetHorizontalAlignment(Alignment eAlignment) { m_eHorizontalAlignment = eAlignment; }
		void SetVerticalAlignment(Alignment eAlignment) { m_eVerticalAlignment = eAlignment; }
		bool IsRenderingAtHorizontalCentre() { return m_eHorizontalAlignment == eAlignment_Centre; }
		bool IsRenderingAtVerticalCentre() { return m_eVerticalAlignment == eAlignment_Centre; }

		Alignment GetHorizontalAlignment() { return m_eHorizontalAlignment; }
		Alignment GetVerticalAlignment() { return m_eVerticalAlignment; }
		virtual void SetMirrorState(MirrorState eMirror) { m_eMirrorState = eMirror; }
		inline MirrorState GetMirrorState() { return m_eMirrorState; }
		virtual void ApplyMirrorValue(MirrorState eMirror);

		static void SetCancelLoad(bool bCancel);
		static bool IsCancellingLoad();

		/** Sets the rotation angle for the sprite. */
		void SetRotationAngle(float fAngle);

		/** Gets the rotation angle for the sprite. */
		float GetRotationAngle() { return m_fRotationAngle; }

		//Anim Time
		float GetAnimTime();
		void SetAnimTime(float fTime);
		void SetRandomAnimTime();

		bool IsMirrored();

		TileAnimation* GetTileAnimation() { return m_pTileAnimation; }

		StringHash GetCurrentAnimationName();
		Surface* GetCurrentTexture();

		virtual void ChangeZoom(float fZoom);

		void ApplyRandomProperties();
		void CopyParentRandomizedProperties(GameObject* pOwner);

		virtual EObjectTypes GetConcreteType() const { return EANIMATION; }

		virtual VERTEX_TYPE* SendForRendering();

		//static void AnimationsLoaded(Object* pObjLoaded, IRenderInstruction* pLoaded);
		void OnAnimationsLoaded(IRenderInstruction* pLoaded);
		void OnSpriteAnimationLoaded(SpriteAnimation* pLoaded);

		void TriggerParticleFromAnimNotify(EmitterComponent* pEmitter, AnimNotify* pNotify);

#ifdef PLAT_PC
		virtual void FixAfterWindowResize();
#endif

	protected:
		virtual void ResetAnimPosition();
		void FinishLoadTileAnimation();

		/** Combined animation data. */
		TileAnimationData* m_pTileAnimationData;

		/** Instance of tile animation data. */
		TileAnimation* m_pTileAnimation;

		//Cached info
		PLAT_BOOL m_bLoaded;
		//PrimType m_iPrimitiveType;

		/* Animation batch this component belongs to. */
		//AnimationBatch* m_pBatch;

		/** The pixel width of the sprite. */
		float m_fWidth;

		/** The pixel height of the sprite. */
		float m_fHeight;

		/** The custom colour of the sprite. */
		Vector4 m_vCustomColour;
		Vector4 m_vRandomColour;

		/** The degree of rotation for this sprite. */
		float m_fRotationAngle;
		float m_fRandomRotationAngle;

		float m_fRandomScaleFactor;
		//POSITION_VECTOR m_vAnimTweenScaleFactor;

		/** The render position */
		POSITION_VECTOR m_vRenderPosition;

		/** Rotation Point Offset */
		POSITION_VECTOR m_vRotationPointOffset;
		POSITION_VECTOR m_vRotationPointScaleOffset;

		/** A flag indicating if the sprite should be drawn or not. */
		// NOTE: Query entity m_bHidden value instead
		//PLAT_BOOL m_bHidden;

		/** Sprite alignment */
		Alignment m_eHorizontalAlignment;
		Alignment m_eVerticalAlignment;

		/** Sprite mirror */
		MirrorState m_eMirrorState;
		int m_iRandomMirror;

		bool m_bScaleTexCoordsWithScale;

#ifdef ON_THE_FLY_ANIMATION_LOADING
		// Variables for delayed load
		LargeBasicString m_sAnimFilePath;
		StringHash m_sInitialAnimName;
		Vector2 m_vInitialScale;
		Vector2 m_vUpperLeftTexCoords;
		Vector2 m_vLowerRightTexCoords;
		PLAT_BOOL m_bRandomizeVertexColours;
		PLAT_BOOL m_bRandomizeInitialFrame;
		//PLAT_BOOL m_bUseCollisionSize;

	private:
		void OnLoadedInitialization();

		float m_fZoomAccumulation;
#endif

		//Engine::Shader

		/** Linked list to all collision components */
		//static List<AnimationComponent> m_aComponents;

		/** Pointer to next component */
		//AnimationComponent* next;
	};

	AnimationComponent* CreateAnimationComponentFromPath(char* sPath, char* sObjectName);
}

#endif	// __EngineAnimationComponent_H__
