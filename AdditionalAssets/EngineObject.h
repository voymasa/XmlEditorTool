#ifndef __EngineObject_H__
#define __EngineObject_H__
#pragma once

#include "EnginePrerequisites.h"
#include "EngineStableHeaders.h"
#include "EngineSerialization.h"
#include "Containers/EngineList.h"
#ifndef DIST
#ifdef PIPELINE_TOOL
#include "tinyxml/tinyxml2.h"
#endif
#endif

#ifdef PIPELINE_TOOL
#include <intrin.h>
#include "EngineStringConverter.h"
#include "EngineString_tmpl.h"
#endif

extern PLAT_BOOL g_bSwapEndian;

/** Currently used in Engine::Object */
#define REGISTER_BASE( BASE_CLASS, CLASSNAME )	\
	public:			\
	CallbackFunctionLookup m_FunctionLookup;	\
	bool CallBoolFunction( StringHash sFuncName )	\
	{			\
	return CALL_FUNCTION_BOOL( sFuncName );	\
	}	\
	bool CallBoolFunction( const char* sFuncName )	\
{			\
	return CALL_FUNCTION_BOOL( ToStringHash(sFuncName) );	\
}	\
	virtual void CallVoidFunction( StringHash sFuncName )	\
	{			\
		CALL_FUNCTION_VOID( sFuncName );	\
}	\
	virtual void CallVoidFunction( const char* sFuncName )	\
{			\
	CALL_FUNCTION_VOID( ToStringHash(sFuncName) );	\
}	\
	int CallIntFunction( StringHash sFuncName )	\
	{			\
		return CALL_FUNCTION_INT( sFuncName );	\
}	\
	int CallIntFunction( const char*  sFuncName )	\
{			\
	return CALL_FUNCTION_INT( ToStringHash(sFuncName) );	\
}	\
	float CallFloatFunction( StringHash sFuncName )	\
	{			\
		return CALL_FUNCTION_FLOAT( sFuncName );	\
}	\
	float CallFloatFunction( const char*  sFuncName )	\
{			\
	return CALL_FUNCTION_FLOAT( ToStringHash(sFuncName) );	\
}	\
	template< typename R >	\
	R CallFunction( StringHash sFuncName )	\
	{			\
		return (R)CALL_FUNCTION( R, sFuncName );	\
	}		\
	template< typename R, typename P1 >	\
	R CallFunction( StringHash sFuncName, P1 param1 )	\
	{			\
		return (R)CALL_FUNCTION_1( R, sFuncName, &param1 );	\
	}		\
	template< typename P1 >	\
	bool CallBoolFunction( StringHash sFuncName, P1 param1 )	\
	{			\
		return CALL_FUNCTION_BOOL_1( sFuncName, &param1 );	\
	}		\
	template< typename R, typename P1, typename P2 >	\
	R CallFunction( StringHash sFuncName, P1 param1, P2 param2 )	\
	{			\
		return (R)CALL_FUNCTION_2( R, sFuncName, &param1, &param2 );	\
	}		\
	template< typename P1, typename P2 >	\
	bool CallBoolFunction( StringHash sFuncName, P1 param1, P2 param2 )	\
	{			\
		return CALL_FUNCTION_BOOL_2( sFuncName, &param1, &param2 );	\
	}		\
	template< typename R, typename P1, typename P2, typename P3 >	\
	R CallFunction( StringHash sFuncName, P1 param1, P2 param2, P3 param3 )	\
	{			\
		return (R)CALL_FUNCTION_3( R, sFuncName, &param1, &param2, &param3 );	\
	}		\
	template< typename P1, typename P2, typename P3 >	\
	bool CallBoolFunction( StringHash sFuncName, P1 param1, P2 param2, P3 param3 )	\
	{			\
		return CALL_FUNCTION_BOOL_3( sFuncName, &param1, &param2, &param3 );	\
	}		\
	template< typename R, typename P1, typename P2, typename P3, typename P4 >	\
	R CallFunction( StringHash sFuncName, P1 param1, P2 param2, P3 param3, P4 param4 )	\
	{			\
		return (R)CALL_FUNCTION_4( R, sFuncName, &param1, &param2, &param3, &param4 );	\
	}		\
	template< typename P1, typename P2, typename P3, typename P4 >	\
	bool CallBoolFunction( StringHash sFuncName, P1 param1, P2 param2, P3 param3, P4 param4 )	\
	{			\
		return CALL_FUNCTION_BOOL_4( sFuncName, &param1, &param2, &param3, &param4 );	\
	}		\
	template< typename R, typename P1, typename P2, typename P3, typename P4, typename P5 >	\
	R CallFunction( StringHash sFuncName, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5 )	\
	{			\
		return (R)CALL_FUNCTION_5( R, sFuncName, &param1, &param2, &param3, &param4, &param5 );	\
	}		\
	template< typename P1, typename P2, typename P3, typename P4, typename P5 >	\
	bool CallBoolFunction( StringHash sFuncName, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5 )	\
	{			\
		return CALL_FUNCTION_BOOL_5( sFuncName, &param1, &param2, &param3, &param4, &param5 );	\
	}		\
	bool DoesFunctionExist( StringHash sFuncName )	\
	{		\
		return m_FunctionLookup.DoesFunctionExist( sFuncName );		\
	}		\
	bool DoesFunctionExist( const char* sFuncName )	\
	{		\
		return m_FunctionLookup.DoesFunctionExist( sFuncName );		\
	}		\
	virtual int GetInitDataSize()	\
	{						\
		return 0;			\
	}						\
	private: 

#define MAX_PIPELINE_DATA 32

#include "EngineFileCollection.h"

namespace Engine
{
	//class FileCollection;
	struct ObjectInitData;
	class GameEngine;
	struct LevelOutputFile;

	typedef int(Engine::GameEngine::* EnumFunction)(const char* sEnumName);
}

extern ObjectInitData* m_pCurrentObjectInitPipeline;

/** This is the main function that you should use if you need to extract the init data from a save file */
ObjectInitData* CreateInitData(FileCollection* pSaveFile);

struct PipelineData
{
	virtual ~PipelineData();

#ifdef PIPELINE_TOOL
	//Remove this constructor once all macros are ported to support the level required format
	PipelineData(char* name, bool bRequired);
	PipelineData(char* name, bool bRequired, bool bLevelRequired);
	virtual bool InitializeSaveData(XMLElement *pElement) = 0;
	virtual void WriteToFile(std::fstream* saveFile) = 0;
	virtual bool ParseEditorProperties( XMLElement* pElement, bool bIgnoreSource ) = 0;
	virtual void InitializeDefault(){}
	int MatchName(const char* sProperty, const char* name);
	virtual void Copy(const PipelineData* pCopy);
	virtual void DerivedCopy(const PipelineData* pCopy) = 0;

	char sName[64];
	bool m_bRequired;
	bool m_bLevelRequired;
	bool m_bSpecified;
	Engine::ObjectInitData* m_pOwnerData;
#else
	PipelineData();
	virtual void InitializeDefault() = 0;
#endif
	virtual void ReadFromFile( FileCollection* saveFile ) = 0;

	PipelineData* next;
};

struct PipelineFloatData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineFloatData(char* sName, float fDefault, bool bRequired);
	PipelineFloatData(char* sName, float fDefault, float fMinValue, float fMaxValue, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
	float fMin;
	float fMax;
#endif
	virtual void ReadFromFile( FileCollection* saveFile );

	operator float() { return fValue; }

	float fValue;
};

template <unsigned int MAX_SIZE>
struct PipelineBasicStringData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineBasicStringData(char* sName, bool bRequired) : PipelineData(sName, bRequired){}

	virtual bool InitializeSaveData(XMLElement *pElement)
	{
		if ( pElement->Attribute(sName) )
		{
			m_bSpecified = true;

			sString = pElement->Attribute(sName);
		}
		return true;
	}

	virtual void WriteToFile(std::fstream* saveFile)
	{
		EXPORT_BASICSTRING_PATH( saveFile, sString);
	}

	virtual bool ParseEditorProperties( XMLElement* pElement, bool bIgnoreSource )
	{
		XMLElement* pProperties = pElement->FirstChildElement("properties");
		if(pProperties)
		{
			pProperties = pProperties->FirstChildElement("property");
			while(pProperties)
			{
				if (MatchName(pProperties->Attribute("name"), sName)==0)
				{
					m_pCurrentObjectInitPipeline->GetFinalOwner()->PropertyMatched(pProperties->Attribute("name"));

					sString = pProperties->Attribute("value");
					m_bSpecified = true;

					return true;
				}
				pProperties = pProperties->NextSiblingElement("property");
			}
		}
		return true;
	}
	
	void DerivedCopy(const PipelineData* pCopy)
	{
		const PipelineBasicStringData* pDerivedCopy = dynamic_cast<const PipelineBasicStringData*>(pCopy);

		sString = pDerivedCopy->sString;
	}

#endif
	virtual void ReadFromFile( FileCollection* saveFile )
	{
		IMPORT_BASICSTRING( saveFile, sString);
	}

	operator StringWrapper<MAX_SIZE>() { return sString; }

	StringWrapper<MAX_SIZE> sString;
};

struct PipelineLevelData : public PipelineBasicStringData<LARGE_BASIC_STRING_LENGTH>
{
#ifdef PIPELINE_TOOL
	PipelineLevelData(char* sName, bool bRequired) : PipelineBasicStringData(sName, bRequired) {}

	virtual void WriteToFile(std::fstream* saveFile);
#endif
};

struct PipelineTexturePathData : public PipelineBasicStringData<LARGE_BASIC_STRING_LENGTH>
{
#ifdef PIPELINE_TOOL
	PipelineTexturePathData(char* sName, bool bRequired);
	virtual void WriteToFile(std::fstream* saveFile);
#endif

};

struct PipelineFilepathData : public PipelineBasicStringData<LARGE_BASIC_STRING_LENGTH>
{
#ifdef PIPELINE_TOOL
	PipelineFilepathData(char* sName, bool bRequired);
	virtual void WriteToFile(std::fstream* saveFile);
#endif
};

struct PipelineIntData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineIntData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
	int iMin;
	int iMax;
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iValue; }

	int iValue;
};

struct PipelineBoolData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineBoolData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator bool() { return bValue; }

	bool bValue;
};

struct PipelineStringHashData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineStringHashData(char* sName, bool bRequired, bool bLevelRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
	bool m_bLower;
	ExtraLargeBasicString m_sString;
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator StringHash() { return hValue; }

	StringHash hValue;
};

struct PipelineGameflagData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineGameflagData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iFlag; }

	int iFlag;
};

struct PipelineLocIDData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineLocIDData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iLocID; }

	int iLocID;
};

struct PipelineAngleData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineAngleData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator Angle() { return aAngle; }

	Angle aAngle;
};

struct PipelineFontData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineFontData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iFont; }

	int iFont;
};

struct PipelineEnumData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineEnumData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iEnumValue; }

	int iEnumValue;
	const char **sEnumNames;
	u32 iNumValues;
	int sDefault;
};

struct PipelineEnumFunctionData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineEnumFunctionData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iEnumValue; }

	int iEnumValue;
	EnumFunction pEnumFunction;
};

struct PipelineVector2Data : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineVector2Data(char* sName, float fX, float fY, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator Engine::Vector2() { return vVec; }

	Engine::Vector2 vVec;
};

struct PipelineVector3Data : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineVector3Data(char* sName, float fX, float fY, float fZ, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator Engine::Vector3() { return vVec; }

	Engine::Vector3 vVec;
};

struct PipelineColourData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineColourData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator Engine::Vector4() { return vColour; }

	Engine::Vector4 vColour;
};
	
struct PipelineRandomData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineRandomData(char* sName, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator RandomContainer() { return vRandom; }

	RandomContainer vRandom;
};

struct PipelineInventoryData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineInventoryData(char* sName, int iDefault, bool bRequired);
	virtual bool InitializeSaveData(XMLElement *pElement);
	virtual void WriteToFile(std::fstream* saveFile);
	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);
	void DerivedCopy(const PipelineData* pCopy);
#endif
	virtual void ReadFromFile(FileCollection* saveFile);

	operator int() { return iItem; }

	int iItem;
};

template<class INITDATATYPE>
struct PipelineObjectsData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineObjectsData(char* sName, bool bRequired) : PipelineData(sName, bRequired) { m_pInitData = 0; }
	virtual bool InitializeSaveData(XMLElement *pElement)
	{
		bool bRetVal = true;

		ObjectInitData* pOwner = m_pCurrentObjectInitPipeline;

		XMLElement* pChild = pElement->FirstChildElement();
		while(pChild)
		{
			LargeBasicString sElementValue = pChild->Value();
			if ( sElementValue.Find(m_sSearchName.ToChar()) != -1 )
			{
				INITDATATYPE* pInitData = 0;
				bool bNew = true;

				// Look for an existing component with this name
				if (pChild->Attribute("Name"))
				{
					LargeBasicString sObjName = pChild->Attribute("Name");

					if ( m_pInitData )
					{
						INITDATATYPE* pCurData = m_pInitData;
						do 
						{
							if (pCurData->sObjectName == sObjName)
							{
								pInitData = pCurData;
								bNew = false;
								break;
							}
							pCurData = pCurData->pNext;
						} while ( pCurData != NULL );
					}
				}

				if(!pInitData)
				{
					LargeBasicString sProperPrefixForSource = m_pCurrentObjectInitPipeline->sProperPrefixForSource;
					pInitData = (INITDATATYPE*)CreateInitDataFromName(ToStringHash(sElementValue.ToChar()), sElementValue.ToChar());
					pInitData->pNext = 0;
					pInitData->sProperPrefixForSource = sProperPrefixForSource;
					pInitData->SetOwner(pOwner);
				}

				bRetVal = bRetVal && pInitData->InitializeSaveData( pChild );

				if(bNew)
				{
					if ( m_pInitData )
					{
						INITDATATYPE* pCurData = m_pInitData;
						while ( pCurData->pNext != NULL )
						{
							pCurData = pCurData->pNext;
						}
						// Store off new next pointer
						pCurData->pNext = pInitData;
					}
					else
					{
						m_pInitData = pInitData;
					}
				}
			}
			pChild = pChild->NextSiblingElement();
		}

		m_pCurrentObjectInitPipeline = pOwner;

		return bRetVal;
	}

	virtual void WriteToFile(std::fstream* saveFile)
	{
		EXPORT_OBJECTS( saveFile, INITDATATYPE, m_pInitData );
	}

	virtual bool ParseEditorProperties( XMLElement* pElement, bool bIgnoreSource )
	{
		XMLElement* pProperties = pElement->FirstChildElement("properties");
		if(pProperties)
		{
			pProperties = pProperties->FirstChildElement("property");
			
			//First check for new components
			while(pProperties)
			{
				BasicString sProperty = pProperties->Attribute("name");
				
				BasicString sNewSearchName = "New-";
				sNewSearchName += m_sSearchName;

				if (sProperty.Find(sNewSearchName)>=0)
				{
					Engine::ObjectInitData* pPrevObjectPipeline = m_pCurrentObjectInitPipeline;

					m_pCurrentObjectInitPipeline->GetFinalOwner()->PropertyMatched(sProperty.ToChar());

					LargeBasicString sProperPrefixForSource = m_pCurrentObjectInitPipeline->sProperPrefixForSource;
					INITDATATYPE* pInitData = (INITDATATYPE*)CreateInitDataFromName(ToStringHash(m_sSearchName.ToChar()), m_sSearchName.ToChar());
					pInitData->pNext = 0;
					pInitData->sProperPrefixForSource = sProperPrefixForSource;

					if (m_pInitData)
					{
						INITDATATYPE* pCurData = m_pInitData;
						while (pCurData->pNext != NULL)
						{
							pCurData = pCurData->pNext;
						}
						// Store off new next pointer
						pCurData->pNext = pInitData;
					}
					else
					{
						m_pInitData = pInitData;
					}

					/*sProperty.Replace("New-", "");
					BasicString sRevisedProperty = sProperty;
					sProperty.GetSubString(sRevisedProperty, 0, sProperty.Find("["));
					pProperties->SetAttribute("name", sRevisedProperty.ToChar());
					pInitData->ParsePipelineEditorProperties(pElement, true);*/
					pInitData->sObjectName = pProperties->Attribute("value");

					m_pCurrentObjectInitPipeline = pPrevObjectPipeline;
				}

				pProperties = pProperties->NextSiblingElement("property");
			}

			pProperties = pElement->FirstChildElement("properties");

			//Then check properties against the new components
			pProperties = pProperties->FirstChildElement("property");
			while (pProperties)
			{
				BasicString sProperty = pProperties->Attribute("name");

				BasicString sNewSearchName = "New-";
				sNewSearchName += m_sSearchName;

				INITDATATYPE* pInitData = m_pInitData;

				while (pInitData)
				{
					if (sProperty.Find(pInitData->sObjectName.ToChar()) != -1)
					{
						pInitData->ParsePipelineEditorProperties(pElement, true);
						break;
					}

					pInitData = pInitData->pNext;
				}

				pProperties = pProperties->NextSiblingElement("property");
			}
		}

		return true;
	}

	void DerivedCopy(const PipelineData* pCopy)
	{
		Assert(m_pInitData==0);
	}

	BasicString m_sSearchName;

#else
	PipelineObjectsData() : PipelineData() { m_pInitData = 0; }
#endif

	~PipelineObjectsData() { DESTROY_IMPORTED_OBJECTS(INITDATATYPE, m_pInitData); }

	virtual void ReadFromFile( FileCollection* saveFile )
	{
		IMPORT_OBJECTS(saveFile, INITDATATYPE, m_pInitData);
	}

	INITDATATYPE* m_pInitData;
};

template<class INITDATATYPE>
struct PipelineInheritanceData : public PipelineData
{
#ifdef PIPELINE_TOOL
	PipelineInheritanceData() : PipelineData("Inheritance", false) 
#else
	PipelineInheritanceData() : PipelineData()
#endif
	{
		Engine::ObjectInitData* pPrevObjectPipeline = m_pCurrentObjectInitPipeline;

		m_pInitData = NEW INITDATATYPE;

		m_pCurrentObjectInitPipeline = pPrevObjectPipeline;
	}

	~PipelineInheritanceData()
	{
		MEMDELETE(m_pInitData);
	}

#ifdef PIPELINE_TOOL
	virtual bool InitializeSaveData(XMLElement *pElement)
	{
		ObjectInitData* pPrevObjectPipeline = m_pCurrentObjectInitPipeline;
		m_pInitData->sProperPrefixForSource = m_pOwnerData->sProperPrefixForSource;
		bool bRet = m_pInitData->InitializeSaveData(pElement);

		m_pCurrentObjectInitPipeline = pPrevObjectPipeline;

		return bRet;
	}

	virtual void WriteToFile(std::fstream* saveFile)
	{
		m_pInitData->sProperPrefixForSource = m_pOwnerData->sProperPrefixForSource;
		m_pInitData->WriteToFile(saveFile);
	}

	virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource)
	{
		return m_pInitData->ParseEditorProperties(pElement, bIgnoreSource);
	}

	void DerivedCopy(const PipelineData* pCopy)
	{
		const PipelineInheritanceData* pDerivedCopy = dynamic_cast<const PipelineInheritanceData*>(pCopy);

		List<PipelineData>::Iterator iter = m_pInitData->m_lPipelineData.GetIterator();
		List<PipelineData>::ConstIterator iter2 = pDerivedCopy->m_pInitData->m_lPipelineData.GetConstIterator();

		for (; !iter.Done(); iter.Next())
		{
			iter.GetValue()->Copy(iter2.GetValue());

			iter2.Next();
		}
	}
#endif

	virtual void ReadFromFile(FileCollection* saveFile)
	{
		//Burn the factory index/object class name since ReadFromFile will expect it to have been read already
#ifdef USE_FACTORY_INDICES
		int iFactoryIndex;
		IMPORT_INT(saveFile, iFactoryIndex);
#else
		
#ifdef USING_LEVEL_EDITOR
		BasicString sClassName;
		IMPORT_BASICSTRING(saveFile, sClassName);
#endif

		StringHash sClassNameHash;
		IMPORT_STRINGHASH(saveFile, sClassNameHash);
#endif

		m_pInitData->ReadFromFile(saveFile);
	}

	virtual void InitializeDefault()
	{
		m_pInitData->InitializeDefault();
	}

	INITDATATYPE* m_pInitData;
};

#ifdef PIPELINE_TOOL
#define PIPELINE_FLOAT_REQUIRED(x, fDefault, bRequired) struct PipelineFloat_##x : public PipelineFloatData \
{ \
	PipelineFloat_##x() : PipelineFloatData(#x, fDefault, bRequired) \
	{ \
	} \
};\
	PipelineFloat_##x x;

#define PIPELINE_FLOAT(x, fDefault) PIPELINE_FLOAT_REQUIRED(x, fDefault, false) 

#define PIPELINE_FLOAT_WITH_MINMAX_REQUIRED(x, fDefault, fMinValue, fMaxValue, bRequired) struct PipelineFloat_##x : public PipelineFloatData \
{ \
	PipelineFloat_##x() : PipelineFloatData(#x, fDefault, fMinValue, fMaxValue, bRequired)   \
	{ \
	} \
};\
	PipelineFloat_##x x;

#define PIPELINE_FLOAT_WITH_MINMAX(x, fDefault, fMinValue, fMaxValue) PIPELINE_FLOAT_WITH_MINMAX_REQUIRED(x, fDefault, fMinValue, fMaxValue, false)

#define PIPELINE_BASICSTRING_REQUIRED(x, sDefault, bRequired) struct PipelineBasicString_##x : public PipelineBasicStringData<BASIC_STRING_LENGTH> \
{ \
	PipelineBasicString_##x() : PipelineBasicStringData<BASIC_STRING_LENGTH>(#x, bRequired) \
	{ \
		sString = sDefault; \
	} \
};\
	PipelineBasicString_##x x;

#define PIPELINE_BASICSTRING(x, sDefault) PIPELINE_BASICSTRING_REQUIRED(x, sDefault, false)

#define PIPELINE_LARGEBASICSTRING_REQUIRED(x, sDefault, bRequired) struct PipelineLargeBasicString_##x : public PipelineBasicStringData<LARGE_BASIC_STRING_LENGTH> \
{ \
	PipelineLargeBasicString_##x() : PipelineBasicStringData<LARGE_BASIC_STRING_LENGTH>(#x, bRequired) \
	{ \
		sString = sDefault; \
	} \
};\
	PipelineLargeBasicString_##x x;

#define PIPELINE_LARGEBASICSTRING(x, sDefault) PIPELINE_LARGEBASICSTRING_REQUIRED(x, sDefault, false)

#define PIPELINE_LEVEL_REQUIRED(x, sDefault, bRequired) struct PipelineLevel_##x : public PipelineLevelData \
{ \
	PipelineLevel_##x() : PipelineLevelData(#x, bRequired) \
	{ \
		sString = sDefault; \
	} \
}; \
	PipelineLevel_##x x;

#define PIPELINE_LEVEL(x, sDefault) PIPELINE_LEVEL_REQUIRED(x, sDefault, false)

#define PIPELINE_FILEPATH_REQUIRED(x, sDefault, bRequired) struct PipelineFilepath_##x : public PipelineFilepathData \
{ \
	PipelineFilepath_##x() : PipelineFilepathData(#x, bRequired) \
	{ \
		sString = sDefault; \
	} \
}; \
	PipelineFilepath_##x x;

#define PIPELINE_FILEPATH(x, sDefault) PIPELINE_FILEPATH_REQUIRED(x, sDefault, false)

#define PIPELINE_TEXTUREPATH_REQUIRED(x, sDefault, bRequired) struct PipelineTexturePath_##x : public PipelineTexturePathData \
{ \
	PipelineTexturePath_##x() : PipelineTexturePathData(#x, bRequired) \
	{ \
		sString = sDefault; \
	} \
}; \
	PipelineTexturePath_##x x;

#define PIPELINE_TEXTUREPATH(x, sDefault) PIPELINE_TEXTUREPATH_REQUIRED(x, sDefault, false)

#define PIPELINE_BASICSTRING_SIZE_REQUIRED(x, sDefault, size, bRequired) struct PipelineBasicString_##x : public PipelineBasicStringData<size> \
{ \
	PipelineBasicString_##x() : PipelineBasicStringData<size>(#x, bRequired) \
	{ \
	sString = sDefault; \
} \
};\
	PipelineBasicString_##x x;

#define PIPELINE_BASICSTRING_SIZE(x, sDefault, size) PIPELINE_BASICSTRING_SIZE_REQUIRED(x, sDefault, size, false)

#define PIPELINE_INT_REQUIRED(x, iDefault, bRequired) struct PipelineInt_##x : public PipelineIntData \
{ \
	PipelineInt_##x() : PipelineIntData(#x, bRequired)  \
	{ \
		iValue = iDefault; \
	} \
};\
	PipelineInt_##x x;

#define PIPELINE_INT(x, iDefault) PIPELINE_INT_REQUIRED(x, iDefault, false)

#define PIPELINE_INT_WITH_MINMAX_REQUIRED(x, iDefault, iMinValue, iMaxValue, bRequired) struct PipelineInt_##x : public PipelineIntData \
{ \
	PipelineInt_##x() : PipelineIntData(#x, bRequired)   \
	{ \
		iValue = iDefault; \
		iMin = iMinValue; \
		iMax = iMaxValue; \
		strcpy_s( sName, 64, #x); \
	} \
};\
	PipelineInt_##x x;

#define PIPELINE_INT_WITH_MINMAX(x, iDefault, iMinValue, iMaxValue) PIPELINE_INT_WITH_MINMAX_REQUIRED(x, iDefault, iMinValue, iMaxValue, false)

#define PIPELINE_BOOL_REQUIRED(x, bDefault, bRequired) struct PipelineBool_##x : public PipelineBoolData \
{ \
	PipelineBool_##x() : PipelineBoolData(#x, bRequired) \
	{ \
		bValue = bDefault; \
	} \
};\
PipelineBool_##x x;

#define PIPELINE_BOOL(x, bDefault) PIPELINE_BOOL_REQUIRED(x, bDefault, false)

#define PIPELINE_STRINGHASH_REQUIRED(x, sDefault, bRequired) struct PipelineStringHash_##x : public PipelineStringHashData \
{ \
	PipelineStringHash_##x() : PipelineStringHashData(#x, bRequired, false) \
	{ \
		hValue = sDefault; \
	} \
};\
	PipelineStringHash_##x x;

#define PIPELINE_STRINGHASH_LEVEL_REQUIRED(x, sDefault, bRequired) struct PipelineStringHash_##x : public PipelineStringHashData \
{ \
	PipelineStringHash_##x() : PipelineStringHashData(#x, false, bRequired) \
	{ \
		hValue = sDefault; \
	} \
};\
	PipelineStringHash_##x x;

#define PIPELINE_STRINGHASH(x, sDefault) PIPELINE_STRINGHASH_REQUIRED(x, sDefault, false)

#define PIPELINE_STRINGHASHLOWER_REQUIRED(x, sDefault, bRequired) struct PipelineStringHash_##x : public PipelineStringHashData \
{ \
	PipelineStringHash_##x() : PipelineStringHashData(#x, bRequired, false) \
	{ \
	hValue = sDefault; \
	m_bLower = true;\
	} \
};\
	PipelineStringHash_##x x;

#define PIPELINE_STRINGHASHLOWER(x, sDefault) PIPELINE_STRINGHASHLOWER_REQUIRED(x, sDefault, false)

#define PIPELINE_GAMEFLAG_REQUIRED(x, iDefault, bRequired) struct PipelineGameflag_##x : public PipelineGameflagData \
{ \
	PipelineGameflag_##x() : PipelineGameflagData(#x, bRequired) \
	{ \
		iFlag = iDefault; \
	} \
};\
	PipelineGameflag_##x x;

#define PIPELINE_GAMEFLAG(x, iDefault) PIPELINE_GAMEFLAG_REQUIRED(x, iDefault, false)

#define PIPELINE_LOCID_REQUIRED(x, iDefault, bRequired) struct PipelineLocID_##x : public PipelineLocIDData \
{ \
	PipelineLocID_##x() : PipelineLocIDData(#x, bRequired) \
	{ \
		iLocID = iDefault; \
	} \
};\
	PipelineLocID_##x x;

#define PIPELINE_LOCID(x, iDefault) PIPELINE_LOCID_REQUIRED(x, iDefault, false)

#define PIPELINE_ANGLE_REQUIRED(x, iDefault, bRequired) struct PipelineAngleID_##x : public PipelineAngleData \
{ \
	PipelineAngleID_##x() : PipelineAngleData(#x, bRequired) \
	{ \
		aAngle = iDefault; \
	} \
};\
	PipelineAngleID_##x x;

#define PIPELINE_ANGLE(x, iDefault) PIPELINE_ANGLE_REQUIRED(x, iDefault, false)

#define PIPELINE_FONT_REQUIRED(x, iDefault, bRequired) struct PipelineFont_##x : public PipelineFontData \
{ \
	PipelineFont_##x() : PipelineFontData(#x, bRequired) \
	{ \
		iFont = iDefault; \
	} \
};\
	PipelineFont_##x x;

#define PIPELINE_ENUM_REQUIRED(x, ppEnumNames, iNumVals, sDefaultVal, bRequired) struct PipelineEnum_##x : public PipelineEnumData \
{ \
	PipelineEnum_##x() : PipelineEnumData(#x, bRequired) \
	{ \
		iEnumValue = sDefaultVal; \
		sEnumNames = ppEnumNames; \
		iNumValues = iNumVals; \
		sDefault = sDefaultVal; \
	} \
};\
PipelineEnum_##x x;

#define PIPELINE_ENUM(x, ppEnumNames, iNumVals, sDefaultVal) PIPELINE_ENUM_REQUIRED(x, ppEnumNames, iNumVals, sDefaultVal, false)

#define PIPELINE_ENUM_FUNCTION_REQUIRED(x, iDefaultVal, pFunction, bRequired) struct PipelineEnumFunction_##x : public PipelineEnumFunctionData \
	{ \
	PipelineEnumFunction_##x() : PipelineEnumFunctionData(#x, bRequired) \
	{ \
		iEnumValue = iDefaultVal; \
		pEnumFunction = pFunction; \
	} \
	};\
	PipelineEnumFunction_##x x;

#define PIPELINE_ENUM_FUNCTION(x, iDefaultVal, pFunction) PIPELINE_ENUM_FUNCTION_REQUIRED(x, iDefaultVal, pFunction, false)

#define PIPELINE_VECTOR2_REQUIRED(VariableName, sDefaultX, sDefaultY, bRequired) struct PipelineVector2_##VariableName : public PipelineVector2Data \
{ \
	PipelineVector2_##VariableName() : PipelineVector2Data(#VariableName, sDefaultX, sDefaultY, bRequired) \
	{ \
	} \
};\
PipelineVector2_##VariableName VariableName;

#define PIPELINE_VECTOR2(VariableName, sDefaultX, sDefaultY) PIPELINE_VECTOR2_REQUIRED(VariableName, sDefaultX, sDefaultY, false)

#define PIPELINE_VECTOR3_REQUIRED(VariableName, sDefaultX, sDefaultY, sDefaultZ, bRequired) struct PipelineVector3_##VariableName : public PipelineVector3Data \
{ \
	PipelineVector3_##VariableName() : PipelineVector3Data(#VariableName, sDefaultX, sDefaultY, sDefaultZ, bRequired) \
	{ \
	} \
};\
PipelineVector3_##VariableName VariableName;

#define PIPELINE_VECTOR3(VariableName, sDefaultX, sDefaultY, sDefaultZ) PIPELINE_VECTOR3_REQUIRED(VariableName, sDefaultX, sDefaultY, sDefaultZ, false)

#define PIPELINE_COLOUR_REQUIRED(VariableName, sDefaultRed, sDefaultGreen, sDefaultBlue, sDefaultAlpha, bRequired) struct PipelineColour_##VariableName : public PipelineColourData \
{ \
	PipelineColour_##VariableName() : PipelineColourData(#VariableName, bRequired) \
	{ \
		vColour.x = sDefaultRed; \
		vColour.y = sDefaultGreen; \
		vColour.z = sDefaultBlue; \
		vColour.w = sDefaultAlpha; \
	} \
};\
PipelineColour_##VariableName VariableName;

#define PIPELINE_COLOUR(VariableName, sDefaultRed, sDefaultGreen, sDefaultBlue, sDefaultAlpha) PIPELINE_COLOUR_REQUIRED(VariableName, sDefaultRed, sDefaultGreen, sDefaultBlue, sDefaultAlpha, false)

#define PIPELINE_RANDOM_REQUIRED(VariableName, sDefaultMin, sDefaultMax, bRequired) struct PipelineRandom_##VariableName : public PipelineRandomData \
{ \
	PipelineRandom_##VariableName() : PipelineRandomData(#VariableName, bRequired) \
	{ \
		vRandom.m_vMinMax[0] = sDefaultMin; \
		vRandom.m_vMinMax[1] = sDefaultMax; \
	} \
};\
PipelineRandom_##VariableName VariableName;

#define PIPELINE_RANDOM(VariableName, sDefaultMin, sDefaultMax) PIPELINE_RANDOM_REQUIRED(VariableName, sDefaultMin, sDefaultMax, false)

#define PIPELINE_OBJECTS_REQUIRED(VariableName, ClassName, SearchName, bRequired) struct PipelineObjects_##VariableName : public PipelineObjectsData<ClassName> \
{ \
	PipelineObjects_##VariableName() : PipelineObjectsData(#VariableName, bRequired) \
	{ \
		m_sSearchName = #SearchName; \
	} \
};\
	PipelineObjects_##VariableName VariableName;

#define PIPELINE_OBJECTS(VariableName, ClassName, SearchName) PIPELINE_OBJECTS_REQUIRED(VariableName, ClassName, SearchName, false)

#define PIPELINE_INVENTORY_REQUIRED(VariableName, iDefault, bRequired) struct PipelineInventory_##VariableName : public PipelineInventoryData \
{ \
	PipelineInventory_##VariableName() : PipelineInventoryData(#VariableName, iDefault, bRequired) \
	{ \
	} \
};\
PipelineInventory_##VariableName VariableName;

#define PIPELINE_INVENTORY(VariableName, iDefault) PIPELINE_INVENTORY_REQUIRED(VariableName, iDefault, false)
#else
#define PIPELINE_FLOAT(x, fDefault) struct PipelineFloat_##x : public PipelineFloatData \
{ \
	void InitializeDefault()\
	{\
		fValue = fDefault;\
	}\
};\
PipelineFloat_##x x

#define PIPELINE_FLOAT_REQUIRED(x, fDefault, bRequired) PIPELINE_FLOAT(x, fDefault)

#define PIPELINE_FLOAT_WITH_MINMAX(x, fDefault, fMinValue, fMaxValue) struct PipelineFloat_##x : public PipelineFloatData \
{ \
	void InitializeDefault()\
	{\
		fValue = fDefault;\
	}\
};\
PipelineFloat_##x x;

#define PIPELINE_FLOAT_WITH_MINMAX_REQUIRED(x, fDefault, fMinValue, fMaxValue, bRequired) PIPELINE_FLOAT_WITH_MINMAX(x, fDefault, fMinValue, fMaxValue)

#define PIPELINE_BASICSTRING(x, sDefault) struct PipelineBasicString_##x : public PipelineBasicStringData<BASIC_STRING_LENGTH> \
{ \
	void InitializeDefault()\
	{\
		sString = sDefault;\
	}\
};\
PipelineBasicString_##x x

#define PIPELINE_BASICSTRING_REQUIRED(x, sDefault, bRequired) PIPELINE_BASICSTRING(x, sDefault)

#define PIPELINE_LARGEBASICSTRING(x, sDefault) struct PipelineLargeBasicString_##x : public PipelineBasicStringData<LARGE_BASIC_STRING_LENGTH> \
{ \
	void InitializeDefault()\
	{\
		sString = sDefault;\
	}\
};\
PipelineLargeBasicString_##x x

#define PIPELINE_LARGEBASICSTRING_REQUIRED(x, sDefault, bRequired) PIPELINE_LARGEBASICSTRING(x, sDefault)

#define PIPELINE_LEVEL_REQUIRED(x, sDefault, bRequired) struct PipelineLevel_##x : public PipelineLevelData \
{ \
	void InitializeDefault() \
	{ \
		sString = sDefault; \
	} \
}; \
	PipelineLevel_##x x;

#define PIPELINE_LEVEL(x, sDefault) PIPELINE_LEVEL_REQUIRED(x, sDefault, false)

#define PIPELINE_FILEPATH_REQUIRED(x, sDefault, bRequired) struct PipelineFilepath_##x : public PipelineFilepathData \
{ \
	void InitializeDefault() \
	{ \
		sString = sDefault; \
	} \
}; \
	PipelineFilepath_##x x;

#define PIPELINE_FILEPATH(x, sDefault) PIPELINE_FILEPATH_REQUIRED(x, sDefault, false)

#define PIPELINE_TEXTUREPATH(x, sDefault) struct PipelineTexturePath_##x : public PipelineTexturePathData \
{ \
	void InitializeDefault()\
	{\
	sString = sDefault;\
	}\
};\
	PipelineTexturePath_##x x

#define PIPELINE_TEXTUREPATH_REQUIRED(x, sDefault, bRequired) PIPELINE_TEXTUREPATH(x, sDefault)

#define PIPELINE_BASICSTRING_SIZE(x, sDefault, size) struct PipelineBasicString_##x : public PipelineBasicStringData<size> \
{ \
	void InitializeDefault()\
	{\
	sString = sDefault;\
	}\
};\
	PipelineBasicString_##x x

#define PIPELINE_BASICSTRING_SIZE_REQUIRED(x, sDefault, size, bRequired) PIPELINE_BASICSTRING_SIZE(x, sDefault, size)

#define PIPELINE_INT(x, iDefault) struct PipelineInt_##x : public PipelineIntData \
{ \
	void InitializeDefault()\
	{\
		iValue = iDefault;\
	}\
};\
	PipelineInt_##x x

#define PIPELINE_INT_REQUIRED(x, iDefault, bRequired) PIPELINE_INT(x, iDefault)

#define PIPELINE_INT_WITH_MINMAX(x, iDefault, iMin, iMax) struct PipelineInt_##x : public PipelineIntData \
{ \
	void InitializeDefault()\
	{\
		iValue = iDefault;\
	}\
};\
	PipelineInt_##x x

#define PIPELINE_INT_WITH_MINMAX_REQUIRED(x, iDefault, iMin, iMax, bRequired) PIPELINE_INT_WITH_MINMAX(x, iDefault, iMin, iMax)

#define PIPELINE_BOOL(x, bDefault) struct PipelineBool_##x : public PipelineBoolData \
{ \
	void InitializeDefault()\
	{\
		bValue = bDefault;\
	}\
};\
PipelineBool_##x x

#define PIPELINE_BOOL_REQUIRED(x, bDefault, bRequired) PIPELINE_BOOL(x, bDefault)

#define PIPELINE_STRINGHASH(x, sDefault) struct PipelineStringHash_##x : public PipelineStringHashData \
{ \
	void InitializeDefault()\
	{\
		hValue = sDefault;\
	}\
};\
PipelineStringHash_##x x

#define PIPELINE_STRINGHASH_REQUIRED(x, sDefault, bRequired) PIPELINE_STRINGHASH(x, sDefault)
#define PIPELINE_STRINGHASH_LEVEL_REQUIRED(x, sDefault, bRequired) PIPELINE_STRINGHASH(x, sDefault)

#define PIPELINE_STRINGHASHLOWER(x, sDefault) struct PipelineStringHash_##x : public PipelineStringHashData \
{ \
	void InitializeDefault()\
	{\
	hValue = sDefault;\
	}\
};\
	PipelineStringHash_##x x

#define PIPELINE_STRINGHASHLOWER_REQUIRED(x, sDefault, bRequired) PIPELINE_STRINGHASHLOWER(x, sDefault)

#define PIPELINE_GAMEFLAG(x, iDefault) struct PipelineGameflag_##x : public PipelineGameflagData \
{ \
	void InitializeDefault()\
	{\
		iFlag = iDefault;\
	}\
};\
PipelineGameflag_##x x

#define PIPELINE_GAMEFLAG_REQUIRED(x, iDefault, bRequired) PIPELINE_GAMEFLAG(x, iDefault)

#define PIPELINE_LOCID(x, iDefault) struct PipelineLocID_##x : public PipelineLocIDData \
{ \
	void InitializeDefault()\
	{\
		iLocID = iDefault;\
	}\
};\
PipelineLocID_##x x

#define PIPELINE_LOCID_REQUIRED(x, iDefault, bRequired) PIPELINE_LOCID(x, iDefault)

#define PIPELINE_ANGLE(x, iDefault) struct PipelineAngle_##x : public PipelineAngleData \
{ \
	void InitializeDefault()\
	{\
		aAngle = iDefault;\
	}\
};\
PipelineAngle_##x x

#define PIPELINE_ANGLE_REQUIRED(x, iDefault, bRequired) PIPELINE_ANGLE(x, iDefault)

#define PIPELINE_FONT(x, iDefault) struct PipelineFont_##x : public PipelineFontData \
{ \
	void InitializeDefault()\
	{\
		iFont = iDefault;\
	}\
};\
PipelineFont_##x x

#define PIPELINE_FONT_REQUIRED(x, iDefault, bRequired) PIPELINE_FONT(x, iDefault)

#define PIPELINE_ENUM(x, sEnumNames, iNumValues, sDefault) struct PipelineEnum_##x : public PipelineEnumData \
{ \
	void InitializeDefault()\
	{\
		iEnumValue = sDefault;\
	}\
};\
PipelineEnum_##x x

#define PIPELINE_ENUM_REQUIRED(x, sEnumNames, iNumValues, sDefault, bRequired) PIPELINE_ENUM(x, sEnumNames, iNumValues, sDefault)

#define PIPELINE_ENUM_FUNCTION(x, iDefault, pFunction) struct PipelineEnumFunction_##x : public PipelineEnumFunctionData \
{ \
	void InitializeDefault()\
	{\
	iEnumValue = iDefault;\
	}\
};\
	PipelineEnumFunction_##x x

#define PIPELINE_ENUM_FUNCTION_REQUIRED(x, iDefault, pFunction, bRequired) PIPELINE_ENUM_FUNCTION(x, iDefault, pFunction)

#define PIPELINE_VECTOR2(VariableName, sDefaultX, sDefaultY) struct PipelineVector2_##VariableName : public PipelineVector2Data \
{ \
	void InitializeDefault()\
	{\
		vVec[0] = sDefaultX;\
		vVec[1] = sDefaultY;\
	}\
};\
PipelineVector2_##VariableName VariableName

#define PIPELINE_VECTOR2_REQUIRED(VariableName, sDefaultX, sDefaultY, bRequired) PIPELINE_VECTOR2(VariableName, sDefaultX, sDefaultY)

#define PIPELINE_VECTOR3(VariableName, sDefaultX, sDefaultY, sDefaultZ) struct PipelineVector3_##VariableName : public PipelineVector3Data \
{ \
	void InitializeDefault()\
	{\
		vVec[0] = sDefaultX;\
		vVec[1] = sDefaultY;\
		vVec[2] = sDefaultZ;\
	}\
};\
PipelineVector3_##VariableName VariableName

#define PIPELINE_VECTOR3_REQUIRED(VariableName, sDefaultX, sDefaultY, sDefaultZ, bRequired) PIPELINE_VECTOR3(VariableName, sDefaultX, sDefaultY, sDefaultZ)

#define PIPELINE_COLOUR(VariableName, sDefaultRed, sDefaultGreen, sDefaultBlue, sDefaultAlpha) struct PipelineColour_##VariableName : public PipelineColourData \
{ \
	void InitializeDefault()\
	{\
		vColour[0] = sDefaultRed;\
		vColour[1] = sDefaultGreen;\
		vColour[2] = sDefaultBlue;\
		vColour[3] = sDefaultAlpha;\
	}\
};\
PipelineColour_##VariableName VariableName

#define PIPELINE_COLOUR_REQUIRED(VariableName, sDefaultRed, sDefaultGreen, sDefaultBlue, sDefaultAlpha, bRequired) PIPELINE_COLOUR(VariableName, sDefaultRed, sDefaultGreen, sDefaultBlue, sDefaultAlpha)

#define PIPELINE_RANDOM(VariableName, sDefaultMin, sDefaultMax) struct PipelineRandom_##VariableName : public PipelineRandomData \
{ \
	void InitializeDefault()\
	{\
		vRandom.m_vMinMax[0] = sDefaultMin;\
		vRandom.m_vMinMax[1] = sDefaultMax;\
	}\
};\
PipelineRandom_##VariableName VariableName

#define PIPELINE_RANDOM_REQUIRED(VariableName, sDefaultMin, sDefaultMax, bRequired) PIPELINE_RANDOM(VariableName, sDefaultMin, sDefaultMax)

#define PIPELINE_OBJECTS(VariableName, ClassName, SearchName) struct PipelineObjects_##VariableName : public PipelineObjectsData<ClassName> \
{ \
	void InitializeDefault()\
	{\
	}\
};\
PipelineObjects_##VariableName VariableName

#define PIPELINE_OBJECTS_REQUIRED(VariableName, ClassName, SearchName, bRequired) PIPELINE_OBJECTS(VariableName, ClassName, SearchName)

#define PIPELINE_INVENTORY(VariableName, iDefault) struct PipelineInventory_##VariableName : public PipelineInventoryData \
{ \
	void InitializeDefault()\
	{\
		iItem = iDefault;\
	}\
};\
PipelineInventory_##VariableName VariableName;

#define PIPELINE_INVENTORY_REQUIRED(VariableName, iDefault, bRequired) PIPELINE_INVENTORY(VariableName, iDefault)
#endif

#define INHERIT(ClassName) struct PipelineInheritance_##ClassName : public PipelineInheritanceData<ClassName> \
{ \
}; \
PipelineInheritance_##ClassName m_Inheritance_##ClassName;

#ifdef USE_3D_POSITION
#define PIPELINE_POSITION_VECTOR(VariableName, sDefaultX, sDefaultY, sDefaultZ) PIPELINE_VECTOR3(VariableName, sDefaultX, sDefaultY, sDefaultZ)
#define PIPELINE_POSITION_VECTOR_REQUIRED(VariableName, sDefaultX, sDefaultY, sDefaultZ, bRequired) PIPELINE_VECTOR3(VariableName, sDefaultX, sDefaultY, sDefaultZ)
#else
#define PIPELINE_POSITION_VECTOR(VariableName, sDefaultX, sDefaultY, sDefaultZ) PIPELINE_VECTOR2(VariableName, sDefaultX, sDefaultY);
#define PIPELINE_POSITION_VECTOR_REQUIRED(VariableName, sDefaultX, sDefaultY, sDefaultZ, bRequired) PIPELINE_VECTOR2(VariableName, sDefaultX, sDefaultY)
#endif

namespace Engine
{
	class Parser;
	class Level;

	/** The InitData structure is used to determine the format of the binary file for this object */
	struct ObjectInitData
	{
		ObjectInitData();
		virtual ~ObjectInitData();

#ifdef PIPELINE_TOOL
		virtual bool InitializeSaveData(XMLElement *pElement);
		virtual bool InitializeSaveData(Parser* pParser, char** ppVariableNames, int iNumVariables);
		virtual bool InitializePipelineSaveData(XMLElement *pElement);
		// ParseEditorProperties is used by level export to import values but ignore an infinite loop of continuously reading source file
		virtual bool ParseEditorProperties(XMLElement* pElement, bool bIgnoreSource);

		// This is to parse editor properties that if specified, we need to parse even for subobjects on an object for which m_bParseLevelEditorFromSubObject is false
		// The primary example at the moment is if UseLevelEditorSizeValues is specified on a sub object's rectangular collision component, we need to have the editor parse those necessarily
		virtual bool ParseNecessaryEditorProperties(XMLElement* pElement, bool bIgnoreSource) { return true; }

		virtual ObjectInitData* FindObject(const char* sName);

		virtual void Copy(const ObjectInitData& object);

		virtual ObjectInitData* GetOwner() { return m_pOwner; }
		virtual ObjectInitData* GetFinalOwner();
		void SetOwner(ObjectInitData* pOwner);
		ObjectInitData* m_pOwner;

#ifdef USING_LEVEL_EDITOR
		bool* m_bPropertyMatched;

		void PrepareProperties();
		void SaveProperties(XMLElement* pElement);
		void FinalizeProperties();

		virtual void VerifyPropertiesMatched();
		void PropertyMatched(const char* sProperty);
		void XMLPropertyMatched(const char* sProperty);
#else
		void PropertyMatched(const char* sProperty) {}
		void XMLPropertyMatched(const char* sProperty) {}
#endif

		virtual bool ParsePipelineEditorProperties( XMLElement* pElement, bool bIgnoreSource);
		virtual void WriteToFile(std::fstream* saveFile );
		virtual void WritePipelineToFile(std::fstream* saveFile);
		virtual bool HasPosition(){ return false; }
		virtual bool ShouldExport(){ return m_bExport; }

		StringHash sObjectNameHash;

		bool m_bExport;

		virtual bool IsIncluding() { return m_bRandomInclusion; }
		bool m_bRandomInclusion;

		Vector2 vRandomizedOffset;
		Vector2 vRandomizedOffsetFromInitData;

		LargeBasicString sRandomSource;
		int m_iRandomSourceIndex;

		int m_iMinRSANumToSpawn;
		int m_iMaxRSANumToSpawn;
		float m_fMinRSASpawnX;
		float m_fMaxRSASpawnX;
		float m_fMinRSASpawnY;
		float m_fMaxRSASpawnY;

#if USING_DYNAMIC_STRINGS
		bool LoadSourceGameObject( BasicString source, ObjectInitData* pSaveData );
#else
		bool LoadSourceGameObject( LargeBasicString source, ObjectInitData* pSaveData );
#endif
#endif
		virtual void ReadFromFile( FileCollection* pSaveFile );
		virtual void ReadPipelineFromFile( FileCollection* saveFile );

		void InitializeDefault();

		virtual void SetPriority(int iPriority){}
		virtual void AddPriority(int iPriority){}

		virtual const char* GetDebugString();

		int iFactoryObjectIndex;
		BasicString sObjectClassName;
		StringHash sObjectClassHashName;
		LargeBasicString sObjectName;

		inline void SetObjectClassName(const char* sName) { 
			sObjectClassName = sName;
			sObjectClassHashName = ToStringHash(sName);
		}

#if USING_DYNAMIC_STRINGS
		BasicString sConfigFile;
		BasicString sSourceFile;
		BasicString sProperPrefixForSource;	// Should not be exported.  used in builders
#else
		LargeBasicString sConfigFile;
		LargeBasicString sSourceFile;
		LargeBasicString sProperPrefixForSource;	// Should not be exported.  used in builders
#endif

		//Can't use a PIPELINE_BOOL for this since it'd get initialized in the constructor before we can set m_pCurrentObjectInitPipeline
		PLAT_BOOL bRegisterFunctions;

		List<PipelineData> m_lPipelineData;

#ifdef USING_LEVEL_EDITOR
#define MAX_PROPERTIES 512

#ifdef PIPELINE_TOOL
		LargeBasicString* m_sExportProperties;
		LargeBasicString* m_sExportValues;

		//We'll make a big array of size MAX_PROPERTIES here so we have room but then delete it to make space, copying over just the number we need into the ExportProperties
		LargeBasicString* m_sProperties;
		LargeBasicString* m_sValues;

		LargeBasicString* m_sXMLProperties;
		int m_iNumXMLProperties;
		bool* m_bXMLPropertyMatched;
#else
		LargeBasicString* m_sProperties;
		LargeBasicString* m_sValues;
#endif
		int m_iNumProperties;
#endif
	};

	/** A basic object in the engine.
	*/
	class Object
	{
		REGISTER_BASE( Object, "Object" )
	public:
		/** Constructor */
		Object( ObjectInitData *pInitData );	// This constructor is for everything else (managers, etc..)
		virtual void FinishInitData( ObjectInitData *pInitData, Level* pLevel=0 ){}

		friend class GameObjectManager;

		/** Destructor */
		virtual ~Object();

		virtual int PostInitialize();
		virtual int Initialize(){ return 1;}
		virtual void Destroy();

		/** Accessors */
#if !defined(DIST) || defined(DIST_DEBUG) || defined(PLAT_PC)
#if !defined(PLAT_DS) || (defined(PLAT_DS) && !defined(USE_DEV_MEM))	// Save Memory
		const LargeBasicString* GetObjectName() const { return &m_sObjectName; }
		const BasicString* GetObjectClassName() const { return &m_sObjectClassName; }
#endif
#endif

#ifdef USING_LEVEL_EDITOR
		LargeBasicString* GetSourceFile() { return &m_sSourceFile;  }
		virtual void SetSourceFile(const char* sSource) { m_sSourceFile = sSource; }
#endif

		StringHash GetObjectNameHash() const { return m_hObjectNameHash; }
		u32 GetGameID() const { return m_uID; }

		//Used primarily in the SpawnEntityScript when we want to give the new entity a name other than the default one in the .src file for the purpose
		//of referencing it in the script
		void SetObjectName(BasicString sNewObjectName);
		void SetObjectNameHash(StringHash sHash){ m_hObjectNameHash = sHash; }

		void SetupResourceDependencies(int iNumDependencies);
		void AddResourceDependency( StringHash sResourceName );
		void RemoveResourceDependency( StringHash sResourceName );
		void DestroyResourceDependencies();
		virtual int GetNumResourceDependencies();
		inline bool IsDestroyed() const { return m_bDestroyed; }

		//void SetResourcesLoadedCallback( void* callbackFunc );

		void RegisterFunctionsImmediate();
		void SetRegisterFunctions(bool bSet){ m_bRegisterFunctions = bSet; }
		bool QueryRegisterFunctions() const { return m_bRegisterFunctions; }

#if defined(USING_LEVEL_EDITOR)
		virtual void SaveProperties(LevelOutputFile& level);

		void SetProperty(LargeBasicString sProperty, const BasicString* sValue);
		void SetProperty(LargeBasicString sProperty, const LargeBasicString* sValue);
		void SetProperty(LargeBasicString sProperty, LargeBasicString sValue);
		void SetProperty(LargeBasicString sProperty, float fValue);
		void SetProperty(LargeBasicString sProperty, int iValue);
		void SetProperty(LargeBasicString sProperty, StringHash hValue);
		void SetBoolProperty(LargeBasicString sProperty, bool bValue);
		void SetProperty(LargeBasicString sProperty, char* sValue);

		void RemovePropertyByNameAndValue(LargeBasicString sProperty, LargeBasicString sValue);
		void RemoveProperty(LargeBasicString sProperty);
#endif
		//Ideally this function shouldn't be public but the problem is that if we need to control which allocator is used for allocations 
		//(notably when spawned using an EntitySpawnerComponent) we need to be able to register the functions outside of PostInitialize too
		virtual void RegisterFunctions();

	protected:
		PLAT_BOOL bFunctionsRegistered;

	private:
		/** ID of this object.  This is a fast handle to objects. */
		u32 m_uID;

#if !defined(DIST) || defined(DIST_DEBUG) || defined(PLAT_PC)
#if !defined(PLAT_DS) || (defined(PLAT_DS) && !defined(USE_DEV_MEM))	// Save Memory
		/** The name of the class that this object is */
		BasicString m_sObjectClassName;

		/** The name of this object..use hash when doing name comparisions */
		LargeBasicString m_sObjectName;
#endif
#endif

		/** The has of the name of this object. To be used for fast comparisions. */
		StringHash m_hObjectNameHash;

		/** What resources is this object waiting for to load before it is ready to be used.  Once loaded, they are removed from this list */
		StringHash* m_aResourceDependencies;
		int m_iNumResourceDependencies;

		PLAT_BOOL m_bDestroyed;
		PLAT_BOOL m_bRegisterFunctions;	// Should this Object register it's functions (i.e. if false, it's light-weight)

#ifdef USING_LEVEL_EDITOR
		LargeBasicString m_sSourceFile;

		LargeBasicString* m_sProperties;
		LargeBasicString* m_sValues;
		int m_iNumProperties;
#endif
	};
}

#endif // __EngineObject_H__
